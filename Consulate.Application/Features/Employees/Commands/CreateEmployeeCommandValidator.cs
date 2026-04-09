using FluentValidation;

namespace Consulate.Application.Features.Employees.Commands
{
    public class CreateEmployeeCommandValidator
    : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Position)
                .NotEmpty();
        }
    }
}
