using Consulate.Application.Common.Exceptions;
using Consulate.Application.Features.Auth.DTOS;
using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using Hangfire;
using MediatR;

namespace Consulate.Application.Features.Auth.Comands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICacheService _cacheService;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository,
            ICacheService cacheService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            _cacheService = cacheService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"user:{request.Email}";

            //  محاولة جلب المستخدم من Redis
            var cachedUser = await _cacheService.GetAsync<UserCacheDto>(cacheKey);

            User user;

            if (cachedUser == null)
            {
                Console.WriteLine(" From DB");

                // جلب من DB
                user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null)
                    throw new UnauthorizedException("Invalid email or password.");

                //  تحويل إلى DTO (لتجنب Circular Reference)
                var userDto = new UserCacheDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    IsVerified = user.IsEmailVerified,
                    Roles = user.UserRoles.Select(x => x.Role.Name).ToList()
                };

                //  تخزين في Redis
                await _cacheService.SetAsync(cacheKey, userDto, TimeSpan.FromMinutes(30));
            }
            else
            {
                Console.WriteLine(" From Redis");

                //  تحويل DTO → Entity
                user = new User
                {
                    Id = cachedUser.Id,
                    Email = cachedUser.Email,
                    PasswordHash = cachedUser.PasswordHash,
                    IsEmailVerified = cachedUser.IsVerified
                };
            }

            //  تحقق من كلمة المرور
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password.");

            //  تحقق من تفعيل الإيميل
            if (!user.IsEmailVerified)
                throw new UnauthorizedException("imad Email not verified.");

            //  إنشاء Access Token
            var accessToken = _jwtService.GenerateAccessToken(user);

            //  إنشاء Refresh Token
            var refreshToken = _jwtService.GenerateRefreshToken();

            //  إرسال إشعار تسجيل دخول (Hangfire)
            BackgroundJob.Enqueue<IEmailService>(x =>
                x.SendLoginNotificationEmail(user.Email));

            //  تخزين Refresh Token
            await _refreshTokenRepository.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                IsUsed = false,
                IsRevoked = false,
                
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            //  النتيجة
            return new AuthResponse(
                accessToken,
                refreshToken
            );
        }
    }
}