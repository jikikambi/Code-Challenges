using CodeChallenge.ApplicationLayer.Tracking.Models;

namespace CodeChallenge.ApplicationLayer.Requests.Services;

public interface ITrackingRequestBase<TInput>
{
    public TInput Input { get; set; }
    public string CorrelationId { get; set; }
    public CanonicalType CanonicalType { get; set; }
    public Event Event { get; set; }
}