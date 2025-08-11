namespace CodeChallenge.DomainLayer.Common;

public interface IEventSourcedAggregate
{
    Guid Id { get; }
    //  Used for optimistic concurrency control.
    //  It tracks the number of events that have been applied to the aggregate since its creation.
    int Version { get; }

    IEnumerable<DomainEvent> GetUncommittedEvents();
    void ClearUncommittedEvents();

    void ApplyEvent(DomainEvent @event);
    void LoadFromHistory(IEnumerable<DomainEvent> history);
}