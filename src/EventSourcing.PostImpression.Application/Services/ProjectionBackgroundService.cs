using System.Text.Json;
using EventSourcing.PostImpression.Application.Contracts.Persistent;
using EventSourcing.PostImpression.Application.Contracts.Services;
using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Domain.Events;
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
            var projectionBackgroundWorker = scope.ServiceProvider.GetRequiredService<IProjectionBackgroundWorker>();
            
            await projectionBackgroundWorker.RunAsync(stoppingToken);
            
            await Task.Delay(30000, stoppingToken);
        }
    }
}