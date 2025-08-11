namespace CodeChallenge.DomainLayer.Common;

public abstract class DomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public Guid AggregateId { get; init; }
}