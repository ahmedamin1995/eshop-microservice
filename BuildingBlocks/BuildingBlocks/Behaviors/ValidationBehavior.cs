using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TResponse : notnull
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));


            var failures = validationResults.Where(er => er.Errors.Any()).SelectMany(er => er.Errors).ToList();


            if (failures.Any())
            {
                logger.LogWarning("Validation failed for request {@request} with errors: {@errors}", request, failures);
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
