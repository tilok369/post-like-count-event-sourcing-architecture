using Carter;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing.PostImpression.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly);
        });

        services.AddCarter();

        return services;
    }
}