using Consulate.Application.Features.Auth.DTOS;
using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using MediatR;
using System.Security.Claims;

namespace Consulate.Application.Features.Auth.Comands.Refresh
{
    public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository)
           
 
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            
        }

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            //استخراج الاكسس توكن رغم انخ منتهي من اليوزر الخاص به
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);

            //استخراج اليوزر اي دي من الكلايمز 
            var userIdString = principal.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userIdString))
                throw new Exception("Invalid token");

            // 🔥 مهم: لأن Id نوعه Guid
            if (!Guid.TryParse(userIdString, out var userId))
                throw new Exception("Invalid user id format");

            //جلب الرفرش توكن من الداتا بيز
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            //التحقق من صحة الرفرش توكن اذا كان موجودا و غير مستخدم و غير منتهي الصلاحية
            if (storedToken == null || storedToken.IsUsed || storedToken.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Invalid token");

            //  جلب المستخدم
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("Invalid user");

            // 4. إنشاء tokens جديدة
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            //  إلغاء التوكن القديم وهو تطبيق لمفهوم الرفرش توكن الواحد لكل مرة (rotation)
            storedToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(storedToken);

            // حفظ الرفرش توكن الجديد في الداتا بيز
            await _refreshTokenRepository.AddAsync(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                IsUsed = false,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            //  إرجاع النتيجة
            return new AuthResponse(newAccessToken, newRefreshToken);
        }
    }
}
