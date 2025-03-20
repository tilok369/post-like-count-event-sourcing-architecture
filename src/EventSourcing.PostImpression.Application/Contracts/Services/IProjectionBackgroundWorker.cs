namespace EventSourcing.PostImpression.Application.Contracts.Services;

public interface IProjectionBackgroundWorker
{
    Task RunAsync(CancellationToken stoppingToken);
}