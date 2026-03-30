using Consulate.Application.Features.Auth.DTOS;
using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using MediatR;

namespace Consulate.Application.Features.Auth.Comands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            // 1. Access Token
            var accessToken = _jwtService.GenerateAccessToken(user);

            // 2. Refresh Token 
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _refreshTokenRepository.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                IsUsed = false,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            // 3. رجّع الاثنين
            return new AuthResponse(
                accessToken,
                refreshToken
            );
        }
    }
}
