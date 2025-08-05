using CodeChallenge.ApplicationLayer.Tracking.Models;
using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.ApplicationLayer.Requests.Services;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public abstract class TrackingRequestCommon<TInput>(CanonicalType canonicalType, 
    Event e,
    TInput input,
    string correlationId)
{
    public string CorrelationId { get; set; } = correlationId;
    public TInput Input { get; set; } = input;
    public Event Event { get; set; } = e;
    public CanonicalType CanonicalType { get; set; } = canonicalType;
}


[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public abstract class TrackingRequestBase<TInput>(CanonicalType canonicalType, Event e, TInput input, string correlationId)
: TrackingRequestCommon<TInput>(canonicalType, e, input, correlationId), ITrackingRequest<TInput>
{ }

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public abstract class TrackingRequestBase<TInput, TResponse>(CanonicalType canonicalType, Event e, TInput input, string correlationId)
: TrackingRequestCommon<TInput>(canonicalType, e, input, correlationId), ITrackingRequest<TInput, TResponse>
{ }