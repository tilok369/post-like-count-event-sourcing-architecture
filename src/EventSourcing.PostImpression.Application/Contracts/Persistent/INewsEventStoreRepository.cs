using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;

namespace EventSourcing.PostImpression.Application.Contracts.Persistent;

public interface INewsEventStoreRepository
{
    Task<DateTime> SaveAsync(Guid newsId, IEnumerable<NewsImpressionBaseEvent> events);
    Task<IEnumerable<NewsImpressionBaseEvent>> GetAsync(Guid newsId);
    Task<DateTime> UpdateProjectionAsync(Guid newsId, IEnumerable<NewsImpressionBaseEvent> events);
    Task<int> GetTotalCountAsync(Guid newsId);
    Task<IEnumerable<NewsEventStore>> GetUnprocessedEventStoreAsync();
    Task UpdateEventStoreAsync(NewsEventStore newsEventStore);
}