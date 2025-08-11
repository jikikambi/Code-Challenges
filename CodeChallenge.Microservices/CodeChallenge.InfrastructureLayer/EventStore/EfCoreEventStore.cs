using CodeChallenge.DomainLayer.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CodeChallenge.InfrastructureLayer.EventStore;

public class EfCoreEventStore(OrderDbContext ctx) : IEventStore
{
    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<DomainEvent> events, CancellationToken ct)
    {
        var serialized = events.Select(e => new StoredEvent
        {
            AggregateId = aggregateId,
            EventType = e.GetType().Name,
            EventData = JsonConvert.SerializeObject(e),
            CreatedAt = e.Timestamp
        });

        ctx.StoredEvents.AddRange(serialized);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<List<DomainEvent>> LoadEventsAsync(Guid aggregateId, CancellationToken ct)
    {
        var raw = await ctx.StoredEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync(ct);

        return raw.Select(e =>
            (DomainEvent)JsonConvert.DeserializeObject(e.EventData, Type.GetType(e.EventType)!)
        ).ToList();
    }
}