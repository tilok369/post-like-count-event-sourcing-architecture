using System.Text.Json;
using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.PostImpression.Infrastructure.Persistent;

public class NewsEventStoreRepository(NewsImpressionDbContext dbContext)
    : INewsEventStoreRepository
{
    public async Task<DateTime> SaveAsync(Guid newsId, IEnumerable<NewsImpressionBaseEvent> events)
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
        return eventEntities?.Last()?.OccurredOn ?? DateTime.MinValue;
    }

    public async Task<IEnumerable<NewsImpressionBaseEvent>> GetAsync(Guid newsId)
    {
        var eventEntities = await dbContext.NewsEventStores
            .Where(e => e.NewsId == newsId)
            .OrderBy(e => e.OccurredOn)
            .ToListAsync();
        
        var assembly = typeof(NewsImpressionBaseEvent).Assembly.FullName;

        return eventEntities.Select(e =>
                (NewsImpressionBaseEvent)JsonSerializer.Deserialize(e.EventData, Type.GetType($"{e.EventType}, {assembly}")))
            .Where(e => e != null)
            .ToList();
    }
    
    public async Task<DateTime> UpdateProjectionAsync(Guid newsId, IEnumerable<NewsImpressionBaseEvent> events)
    {
        var newEntity = false;
        var summary = await dbContext.NewsImpressionSummaries.FindAsync(newsId);
        if (summary == null)
        {
            newEntity = true;
            summary = new NewsImpressionSummary { NewsId = newsId };
        }

        foreach (var @event in events)
        {
            if (@event is NewsLikedEvent) 
                summary.TotalLikes++;
            if (@event is NewsLikeRemovedEvent)
                summary.TotalLikes = Math.Max(0, summary.TotalLikes - 1);
        }
        
        summary.UpdatedOn = DateTime.UtcNow;

        if (newEntity)
            dbContext.NewsImpressionSummaries.Add(summary);
        else
            dbContext.NewsImpressionSummaries.Update(summary);
        await dbContext.SaveChangesAsync();
        return summary?.UpdatedOn ?? DateTime.MinValue;
    }

    public async Task<int> GetTotalCountAsync(Guid newsId)
    {
        var summary = await dbContext.NewsImpressionSummaries.FindAsync(newsId);
        return summary?.TotalLikes ?? 0;
    }
}