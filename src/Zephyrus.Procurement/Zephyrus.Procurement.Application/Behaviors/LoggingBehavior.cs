using MediatR;
using Microsoft.Extensions.Logging;

namespace Zephyrus.Procurement.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse> where TResponse : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[{@DateTimeUtc}] Handling request: {@request}", DateTime.UtcNow, request);

        try
        {
            var response = await next(cancellationToken);
            logger.LogInformation("[{@DateTimeUtc}] Completed request: {@request} with response: {@response}", DateTime.UtcNow, request, response);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{@DateTimeUtc}] Request failed: {@request}", DateTime.UtcNow, request);
            throw;
        }
    }
}
