using AutoMapper;
using Consulate.Application.Common.Pagination;
using Consulate.Application.DTOs;
using Consulate.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Employees.Queries
{
    public class GetAllEmployeesQueryHandler
        : IRequestHandler<GetAllEmployeesQuery, PagedResult<EmployeeDto>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllEmployeesQueryHandler(
            IEmployeeRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<EmployeeDto>> Handle(
            GetAllEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var employees = await _repository.GetAllAsync();

            var totalCount = employees.Count();

            var pagedEmployees = employees
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var mappedEmployees = _mapper.Map<List<EmployeeDto>>(pagedEmployees);

            return new PagedResult<EmployeeDto>
            {
                Items = mappedEmployees,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
