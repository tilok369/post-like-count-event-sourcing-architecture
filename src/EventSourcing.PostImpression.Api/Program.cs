using EventSourcing.PostImpression.Infrastructure.Persistent;
using Carter;
using EventSourcing.PostImpression.Application;
using EventSourcing.PostImpression.Application.Consumers;
using EventSourcing.PostImpression.Application.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddHostedService<ProjectionBackgroundService>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<NewsImpressionAddedConsumer>();
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost",  5672,"/", host =>
        {
            host.Username("guest");
            host.Password("RmU53r!23");
        });
        cfg.ConfigureEndpoints(context);
        cfg.ReceiveEndpoint("news-impression-queue", e =>
        {
            e.ConfigureConsumer<NewsImpressionAddedConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCarter();

app.Run();