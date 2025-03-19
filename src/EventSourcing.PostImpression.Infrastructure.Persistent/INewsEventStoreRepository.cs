using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;

namespace EventSourcing.PostImpression.Infrastructure.Persistent;

public interface INewsEventStoreRepository
{
    Task<DateTime> SaveAsync(Guid newsId, IEnumerable<NewsImpressionBaseEvent> events);
    Task<IEnumerable<NewsImpressionBaseEvent>> GetAsync(Guid newsId);
    Task<DateTime> UpdateProjectionAsync(Guid newsId, IEnumerable<NewsImpressionBaseEvent> events);
    Task<int> GetTotalCountAsync(Guid newsId);
}