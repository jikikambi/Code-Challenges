using CodeChallenge.ApplicationLayer.Exceptions;
using CodeChallenge.ApplicationLayer.Requests.Extensions;
using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.ApplicationLayer.Behaviors;

/// <summary>
/// Is an MediatR pipeline behavior that's used for cross-cutting concerns (logging, tracking, validation):
/// - Tracks request processing status (Pending, Processed, Error).
/// - Logs and handles exceptions.
/// - Sends tracking events to ITrackingService.
/// - Supports dependency injection (ILogger, ITrackingService).
/// - Works generically for different request types.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <param name="logger"></param>
/// <param name="trackingService"></param>
public class TrackingBehavior<TRequest, TResponse, TInput>(ILogger<TrackingBehavior<TRequest, TResponse, TInput>> logger, ITrackingService trackingService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITrackingRequestBase<TInput>
{

    /// <summary>
    /// - Executes the request pipeline.
    /// - Calls next() to invoke the next handler in the pipeline.
    /// - Handles different exception types.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        request.SetPending();
        request.AddInComingData(request.Input);

        try
        {
            var response = await next();
            request.SetProcessed();
            return response;
        }
        catch (FunctionalDataException functionalDataException)
        {
            logger.LogWarning(functionalDataException, "[{RequestType}] Functional error occurred: {Message}", typeof(TRequest).Name, functionalDataException.Message);
            request.SetFunctionalError(functionalDataException);
            return default!;
        }
        catch (ValidationException validationException)
        {
            logger.LogError(validationException, "[{RequestType}] Validation error: {Message}", typeof(TRequest).Name, validationException.Message);
            request.SetFunctionalError(validationException);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{RequestType}] Unhandled exception", typeof(TRequest).Name);
            request.SetTechnicalError(ex);
            throw;
        }
        finally
        {
            await trackingService.AddEventAsync<TRequest, TInput>(request, cancellationToken);
        }
    }
}