using ExpenseTracker.Application.Common.Errors.Controls;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ExpenseTracker.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest>? _validator;

        // default initialized as null in case a certain request doesn't have a validator, and if so, in the Handle will just go to the next one
        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next();
            }
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors
                .ConvertAll(validationFailure =>
                    new ValidationFailure(validationFailure.PropertyName, validationFailure.ErrorMessage));

            throw new ValidationException(errors);
        }
    }
}
