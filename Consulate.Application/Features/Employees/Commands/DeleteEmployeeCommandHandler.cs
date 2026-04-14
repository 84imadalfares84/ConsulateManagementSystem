using Consulate.Application.Common.Exceptions;
using Consulate.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Employees.Commands
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly IEmployeeRepository _repository;

        public DeleteEmployeeCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(request.Id);

            if (employee == null)
                throw new NotFoundException("Employee was not found.");

            employee.IsDeleted = true;

            await _repository.UpdateAsync(employee);

            return Unit.Value;
        }
    }
}
