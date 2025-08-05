using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Models;

namespace CodeChallenge.ApplicationLayer.Tracking.Services;

public class TrackingService(Serilog.ILogger logger) : ITrackingService
{
    private readonly Serilog.ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task AddEventAsync<TRequest, TInput>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : ITrackingRequestBase<TInput>
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        var logData = new EventChain
        {
            Id = request.CorrelationId,
            CorrelationId = request.CorrelationId,
            CanonicalType = request.CanonicalType,
            Events = [request.Event]
        };

        using(Serilog.Context.LogContext.PushProperty("IsTracking", true))
       // using (Serilog.Context.LogContext.PushProperty("CorrelationId", request.CorrelationId))
        {
            _logger.Information("Tracking event: {@LogData}", logData);
        }        

        return Task.CompletedTask;
    }
}
