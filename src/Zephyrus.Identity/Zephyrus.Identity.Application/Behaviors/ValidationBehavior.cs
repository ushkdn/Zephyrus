using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Zephyrus.Identity.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse> where TResponse : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[{@DateTimeUtc}] Validating request: {@request}", DateTime.UtcNow, request);
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var failures = validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();

        if (failures.Count != 0)
        {
            logger.LogError("[{@DateTimeUtc}] Validation failed for request: {@request}, failures: {@failures}", DateTime.UtcNow, request, failures);
            throw new ValidationException("Validation failed", failures);
        }

        return await next(cancellationToken);
    }
}