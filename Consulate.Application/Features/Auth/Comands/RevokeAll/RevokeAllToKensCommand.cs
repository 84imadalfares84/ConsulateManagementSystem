using MediatR;


namespace Consulate.Application.Features.Auth.Comands.RevokeAll
{
    public class RevokeAllToKensCommand(Guid UserId) : IRequest<Unit>
    {
        public Guid UserId { get; } = UserId;
    }
    
}
