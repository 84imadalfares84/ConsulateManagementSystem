using AutoMapper;
using Consulate.Application.Common.Exceptions;
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
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(request.Id);
            if (employee == null)
                throw new NotFoundException("Employee was not found.");

            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}
