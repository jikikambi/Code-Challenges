using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Models;
using Microsoft.Extensions.Options;
using Nest;

namespace CodeChallenge.ApplicationLayer.Tracking.Services;

public class TrackingService(Serilog.ILogger logger,
    IElasticClient elasticClient,
    IOptions<TrackingOptions> options) : ITrackingService
{

    public async Task AddEventAsync<TRequest, TInput>(TRequest request, CancellationToken cancellationToken = default)
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

        // 1) Technical/operational log (Serilog)
        using (Serilog.Context.LogContext.PushProperty("IsTracking", true))
        {
            logger.Information("Tracking event: {@LogData}", logData);
        }

        // 2) Domain tracking document (Elasticsearch)
        var indexName = $"{options.Value.IndexPrefix}-{DateTime.UtcNow:yyyy.MM.dd}";
        var indexRequest = new IndexRequest<EventChain>(logData, indexName)
        {
            Pipeline = null
        };

        var response = await elasticClient.IndexAsync(indexRequest, cancellationToken);
        if (!response.IsValid)
        {
            logger.Error(response.OriginalException,
                "Failed to index tracking document to Elasticsearch (index: {Index}). ServerError: {ServerError}",
                indexName, response.ServerError?.ToString());
        }
    }
}