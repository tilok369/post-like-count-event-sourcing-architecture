using System.Text.Json;
using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.PostImpression.Infrastructure.Persistent;

public class NewsEventStoreRepository(NewsImpressionDbContext dbContext)
    : INewsEventStoreRepository
{
    public async Task SaveAsync(Guid newsId, IEnumerable<NewsImpressionBaseEvent> events)
    {
        var eventEntities = events.Select(e => 
            new NewsEventStore
        {
            NewsId = newsId,
            EventType = e.GetType().ToString(),
            EventData = JsonSerializer.Serialize(e),
            OccurredOn = e.OccurredOn
        });

        await dbContext.NewsEventStores.AddRangeAsync(eventEntities);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<NewsImpressionBaseEvent>> GetAsync(Guid newsId)
    {
        var eventEntities = await dbContext.NewsEventStores
            .Where(e => e.NewsId == newsId)
            .OrderBy(e => e.OccurredOn)
            .ToListAsync();

        return eventEntities.Select(e =>
                JsonSerializer.Deserialize(e.EventData, Type.GetType($"{e.EventType}")!) as NewsImpressionBaseEvent)
            .Where(e => e != null)
            .ToList();
    }
}