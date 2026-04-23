using MediatR;
using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Auth.Comands.RevokeAll
{
    public class RevokeAllToKensHandler : IRequestHandler<RevokeAllToKensCommand,Unit>   
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RevokeAllToKensHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<Unit> Handle(RevokeAllToKensCommand request, CancellationToken cancellationToken)
        {
            var tokens = await _refreshTokenRepository.GetByUserIdAsync(request.UserId);//جلب كل التوكنات اللي تخص اليوزر ده

            if (tokens != null && tokens.Any())
            {
                foreach (var token in tokens)
                {
                    token.IsRevoked = true;
                    token.RevokedAt = DateTime.UtcNow;
                }
                await _refreshTokenRepository.UpdateRangeAsync(tokens);
            }
            return Unit.Value;
        }

        
    }
}
