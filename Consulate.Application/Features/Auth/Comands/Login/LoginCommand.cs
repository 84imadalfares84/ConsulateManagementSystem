using Consulate.Application.Features.Auth.DTOS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Auth.Comands.Login
{
    public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponse>;
}
