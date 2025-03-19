using System.Text.Json;
using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;
using EventSourcing.PostImpression.Infrastructure.Persistent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSourcing.PostImpression.Application.Services;

public class ProjectionBackgroundService(IServiceScopeFactory serviceScopeFactory): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<INewsEventStoreRepository>();
            
            var unProcessedEventStore = await repository.GetUnprocessedEventStoreAsync();

            foreach (var eventStore in unProcessedEventStore)
            {
                var @event = ToEvent(eventStore);
                var processedOn = await repository.UpdateProjectionAsync(@event.NewsId, [@event]);
                if(processedOn == DateTime.MinValue)
                    continue;
                eventStore.ProcessedOn = processedOn;
                eventStore.Processed = true;
                await repository.UpdateEventStoreAsync(eventStore);
                
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    private NewsImpressionBaseEvent ToEvent(NewsEventStore eventStore)
    {
        var assembly = typeof(NewsImpressionBaseEvent).Assembly.FullName;
        return (NewsImpressionBaseEvent)JsonSerializer.Deserialize(eventStore.EventData,
            Type.GetType($"{eventStore.EventType}, {assembly}"));
    }
}