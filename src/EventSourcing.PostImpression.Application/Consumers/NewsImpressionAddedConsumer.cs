using System.Text.Json;
using EventSourcing.PostImpression.Application.Consumers.Models;
using EventSourcing.PostImpression.Application.Contracts.Persistent;
using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;
using MassTransit;

namespace EventSourcing.PostImpression.Application.Consumers;

public class NewsImpressionAddedConsumer(INewsEventStoreRepository repository): IConsumer<NewsImpression>
{
    
    
    public async Task Consume(ConsumeContext<NewsImpression> context)
    {
        var @event = ToEvent(context.Message.EventType, context.Message.EventData);
        var processedOn = await repository.UpdateProjectionAsync(@event.NewsId, [@event]);
    }
    
    private NewsImpressionBaseEvent ToEvent(string eventType, string eventData)
    {
        var assembly = typeof(NewsImpressionBaseEvent).Assembly.FullName;
        return (NewsImpressionBaseEvent)JsonSerializer.Deserialize(eventData,
            Type.GetType($"{eventType}, {assembly}"));
    }
}