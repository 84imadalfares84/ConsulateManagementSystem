using AutoMapper;
using Consulate.Application.DTOs;
using Consulate.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Employees.Commands
{
    public class UpdateEmployeeCommandHandler
        : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IEmployeeRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper=mapper;
        }
        public async Task<EmployeeDto> Handle(
            UpdateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(request.Id);
            if (employee == null)
                throw new Exception("Employee not found");
            employee.FullName = request.Name;
            employee.Email = request.Email;
            employee.Position = request.Position;

            await _repository.UpdateAsync(employee);

            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}
