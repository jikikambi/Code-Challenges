using CodeChallenge.DomainLayer.Common;

namespace CodeChallenge.InfrastructureLayer.Services;

/// <summary>
/// Is the abstraction over your event store persistence.
/// It is used by your aggregates (via the Application or Infrastructure layer) to:
/// - Save new DomainEvent instances when something changes.
/// - Load past events to rebuild an aggregate’s state.
/// </summary>
/// <typeparam name="TAggregate"></typeparam>
public interface IEventStoreRepository<TAggregate> where TAggregate : IEventSourcedAggregate
{
    Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    Task<TAggregate> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default);
}