using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;

namespace EventSourcing.PostImpression.Infrastructure.Persistent;

public interface INewsEventStoreRepository
{
    Task SaveAsync(Guid newsId, IEnumerable<NewsImpressionBaseEvent> events);
    Task<IEnumerable<NewsImpressionBaseEvent>> GetAsync(Guid newsId);
}