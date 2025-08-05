using CodeChallenge.ApplicationLayer.Requests.Services;

namespace CodeChallenge.ApplicationLayer.Tracking.Services;

public interface ITrackingService
{
    Task AddEventAsync<TRequest, TInput>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : ITrackingRequestBase<TInput>;
}