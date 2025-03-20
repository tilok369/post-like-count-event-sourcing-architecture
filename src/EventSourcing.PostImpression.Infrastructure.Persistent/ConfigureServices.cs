using EventSourcing.PostImpression.Application.Contracts.Persistent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing.PostImpression.Infrastructure.Persistent;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NewsImpressionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString"))
        );

        services.AddScoped<INewsEventStoreRepository, NewsEventStoreRepository>();
        
        return services;
    }
}