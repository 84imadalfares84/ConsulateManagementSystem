using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Auth.Comands.EmailVerification
{
    public record VerifyEmailCommand(string Token) : IRequest<string>;
}

