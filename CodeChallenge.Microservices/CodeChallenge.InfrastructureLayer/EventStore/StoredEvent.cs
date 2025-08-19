namespace CodeChallenge.InfrastructureLayer.EventStore;

/// <summary>
/// Persistence representation in the event store.
/// </summary>
public class StoredEvent
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string EventType { get; set; }
    public string EventData { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
}