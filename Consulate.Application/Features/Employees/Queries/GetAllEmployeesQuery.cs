using Consulate.Application.Common.Pagination;
using Consulate.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Employees.Queries
{
       public record GetAllEmployeesQuery(int PageNumber, int PageSize)
        : IRequest<PagedResult<EmployeeDto>>;
}
