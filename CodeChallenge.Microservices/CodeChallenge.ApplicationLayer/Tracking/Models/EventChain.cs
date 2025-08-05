using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.ApplicationLayer.Tracking.Models;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public class EventChain
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;
    public string CorrelationId { get; set; } = default!;
    public EventChainError Error { get; set; }
    public CanonicalType CanonicalType { get; set; }
    public List<Event> Events { get; set; } = default!;
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}