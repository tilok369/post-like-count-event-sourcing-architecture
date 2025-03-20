using Carter;
using EventSourcing.PostImpression.Application.Contracts.Services;
using EventSourcing.PostImpression.Application.Services;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing.PostImpression.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProjectionBackgroundWorker, ProjectionBackgroundWorker>();
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly);
        });

        services.AddCarter();

        return services;
    }
}