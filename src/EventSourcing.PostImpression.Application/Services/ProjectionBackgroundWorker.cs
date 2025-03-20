using System.Text.Json;
using EventSourcing.PostImpression.Application.Consumers.Models;
using EventSourcing.PostImpression.Application.Contracts.Persistent;
using EventSourcing.PostImpression.Application.Contracts.Services;
using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;
using MassTransit;

namespace EventSourcing.PostImpression.Application.Services;

public class ProjectionBackgroundWorker(
    INewsEventStoreRepository eventStoreRepository,
    IPublishEndpoint publisherEndpoint
    ): IProjectionBackgroundWorker
{
    public async Task RunAsync(CancellationToken stoppingToken)
    {
        var unProcessedEventStore = await eventStoreRepository.GetUnprocessedEventStoreAsync();

        foreach (var eventStore in unProcessedEventStore)
        {
            await publisherEndpoint.Publish(new NewsImpression(eventStore.EventType, eventStore.EventData), stoppingToken);
            eventStore.ProcessedOn = DateTime.Now;
            eventStore.Processed = true;
            await eventStoreRepository.UpdateEventStoreAsync(eventStore);
        }
    }
}