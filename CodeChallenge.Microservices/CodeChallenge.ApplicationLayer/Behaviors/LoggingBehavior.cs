using CodeChallenge.ApplicationLayer.Requests.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.ApplicationLayer.Behaviors;

/// <summary>
/// Is an MediatR pipeline behavior that is used for cross-cutting concerns like logging:
/// - Adds structured logging to request handling.To ensure all logs for this request include these identifiers and also best practice in distributed systems (tracing across microservices).
///   - "CorrelationId" → Unique ID to track requests.
///   - "Service" → Identifies which service initiated the request.
///   - "Operation" → Identifies which operation the request is performing.
/// - Logs the request’s lifecycle (start and end).
/// - Includes contextual metadata (CorrelationId, ServiceId, OperationId) in logs.
/// - Handles and logs exceptions.
/// </summary>
/// <typeparam name="TRequest">Must implement ITrackingRequestBase<TInput> → Ensures that requests have CorrelationId, Event.ServiceId, and Event.OperationId.</typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <param name="logger"></param>
[ExcludeFromCodeCoverage(Justification = "It will be tested later")]
public class LoggingBehavior<TRequest, TResponse, TInput>(ILogger<LoggingBehavior<TRequest, TResponse, TInput>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITrackingRequestBase<TInput>
{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = request.CorrelationId,
            ["Service"] = request.Event.ServiceId.ToString(),
            ["Operation"] = request.Event.OperationId,
        }))
        {
            try
            {
                logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);
                var response = await next();
                logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling {RequestName}", typeof(TRequest).Name);
                throw;
            }
        }
    }
}