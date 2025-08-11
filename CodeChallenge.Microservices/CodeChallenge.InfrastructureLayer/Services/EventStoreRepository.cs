using CodeChallenge.DomainLayer.Common;
using CodeChallenge.InfrastructureLayer.EventStore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CodeChallenge.InfrastructureLayer.Services;

public class EventStoreRepository<TAggregate>(OrderDbContext ctx) : IEventStoreRepository<TAggregate>
    where TAggregate : IEventSourcedAggregate
{
    public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var uncommittedEvents = aggregate.GetUncommittedEvents().ToList();

        if (!uncommittedEvents.Any())
            return;

        int expectedVersion = aggregate.Version - uncommittedEvents.Count;

        var existingEventsCount = await ctx.StoredEvents
            .CountAsync(e => e.AggregateId == aggregate.Id, cancellationToken);

        if (existingEventsCount != expectedVersion + 1 && existingEventsCount != expectedVersion)
            throw new InvalidOperationException("Concurrency conflict detected.");

        int version = expectedVersion;

        foreach (var domainEvent in uncommittedEvents)
        {
            version++;

            var storedEvent = new StoredEvent
            {
                Id = Guid.NewGuid(),
                AggregateId = aggregate.Id,
                EventType = domainEvent.GetType().AssemblyQualifiedName ?? domainEvent.GetType().FullName ?? throw new InvalidOperationException("Event type name missing"),
                EventData = JsonConvert.SerializeObject(domainEvent),
                Version = version,
                CreatedAt = DateTime.UtcNow
            };

            ctx.StoredEvents.Add(storedEvent);
        }

        await ctx.SaveChangesAsync(cancellationToken);

        aggregate.ClearUncommittedEvents();
    }

    public async Task<TAggregate> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default)
    {
        var storedEvents = await ctx.StoredEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        if (!storedEvents.Any())
            return default!;

        var domainEvents = new List<DomainEvent>();

        foreach (var storedEvent in storedEvents)
        {
            var eventType = Type.GetType(storedEvent.EventType) ?? throw new InvalidOperationException($"Event type '{storedEvent.EventType}' not found.");
            var domainEvent = (DomainEvent)JsonConvert.DeserializeObject(storedEvent.EventData, eventType)!;
            domainEvents.Add(domainEvent);
        }

        // Create the aggregate via reflection (works with private constructor)
        var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), nonPublic: true)!;
        aggregate.LoadFromHistory(domainEvents);

        return aggregate;
    }
}