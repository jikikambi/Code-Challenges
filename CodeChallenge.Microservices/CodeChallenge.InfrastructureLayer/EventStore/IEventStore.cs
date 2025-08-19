using CodeChallenge.DomainLayer.Common;

namespace CodeChallenge.InfrastructureLayer.EventStore;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<DomainEvent> events, CancellationToken ct);
    Task<List<DomainEvent>> LoadEventsAsync(Guid aggregateId, CancellationToken ct);
}