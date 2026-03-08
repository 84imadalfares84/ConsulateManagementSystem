using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Employees.Commands
{
    public record CreateEmployeeCommand(
    string FullName,
    string Email,
    string Position
) : IRequest<Guid>;
}
