using Consulate.Application.Features.Auth.DTOS;
using MediatR;

namespace Consulate.Application.Features.Auth.Comands.Refresh
{

    public class RefreshTokenCommand : IRequest<AuthResponse>
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
