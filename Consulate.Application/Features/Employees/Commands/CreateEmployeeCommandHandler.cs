using AutoMapper;
using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Employees.Commands
{
    public class CreateEmployeeCommandHandler :IRequestHandler<CreateEmployeeCommand, Guid>
    {

        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(
            IEmployeeRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(
            CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Employee>(request);

            await _repository.AddAsync(employee);

            return employee.Id;
        }
    }
}
