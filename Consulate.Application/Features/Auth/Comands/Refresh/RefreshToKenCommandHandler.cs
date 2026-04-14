using System.Security.Claims;
using Consulate.Application.Common.Exceptions;
using Consulate.Application.Features.Auth.DTOS;
using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using MediatR;

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
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);

            var userIdString = principal.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
                throw new UnauthorizedException("Invalid access token.");

            if (!Guid.TryParse(userIdString, out var userId))
                throw new BadRequestException("Invalid user id format.");

            var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (storedToken == null
                || storedToken.IsUsed
                || storedToken.IsRevoked
                || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new UnauthorizedException("Invalid user.");

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            storedToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(storedToken);

            await _refreshTokenRepository.AddAsync(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                IsUsed = false,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            return new AuthResponse(newAccessToken, newRefreshToken);
        }
    }
}
