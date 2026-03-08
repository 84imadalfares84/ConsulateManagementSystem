using FluentValidation;
using MediatR;

namespace Consulate.Application.Common;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    // This class is a MediatR pipeline behavior that performs validation on incoming requests using FluentValidation.
    //Generic class for apply any type of request and response, and it implements the IPipelineBehavior interface from MediatR.
    //IPieplineBehavior is an interface from MediatR
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any()) //Check if there are any validators for the incoming request type
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);
        }

        return await next();//.نجاح التحقق من صحة الطلب، يتم تمرير الطلب إلى المعالج التالي في السلسلة.
    }
}
