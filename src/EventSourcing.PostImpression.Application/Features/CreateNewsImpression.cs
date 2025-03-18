using Carter;
using EventSourcing.PostImpression.Domain.Entities;
using EventSourcing.PostImpression.Infrastructure.Persistent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.PostImpression.Application.Features;

public static class CreateNewsImpression
{
    public record NewsLikeCommand(Guid Id, int UserId) : IRequest<NewsLikeResponse>;
    public record NewsLikeRemoveCommand(Guid Id, int UserId) : IRequest<NewsLikeRemoveResponse>;

    public record NewsLikeResponse(bool Success, DateTime LikedAt);
    public record NewsLikeRemoveResponse(bool Success, DateTime LikedAt);
    
    
    public class NewsLikeCommandHandler(INewsEventStoreRepository eventStoreRepository)
        : IRequestHandler<NewsLikeCommand, NewsLikeResponse>
    {
        public async Task<NewsLikeResponse> Handle(NewsLikeCommand request, CancellationToken cancellationToken)
        {
            var news = News.Create(request.Id);
            news.Like(request.UserId);
            var occurredOn = await eventStoreRepository.SaveAsync(request.Id, news.GetEvents());
            return new NewsLikeResponse(occurredOn != DateTime.MinValue, occurredOn);
        }
    }
    
    public class NewsLikeRemoveCommandHandler(INewsEventStoreRepository eventStoreRepository)
        : IRequestHandler<NewsLikeRemoveCommand, NewsLikeRemoveResponse>
    {
        public async Task<NewsLikeRemoveResponse> Handle(NewsLikeRemoveCommand request, CancellationToken cancellationToken)
        {
            var news = News.Create(request.Id);
            news.RemoveLike(request.UserId);
            var occurredOn = await eventStoreRepository.SaveAsync(request.Id, news.GetEvents());
            return new NewsLikeRemoveResponse(occurredOn != DateTime.MinValue, occurredOn);
        }
    }
}

public class CreateNewsImpressionEndpoints: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("news/like", async (CreateNewsImpression.NewsLikeCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        });
        
        app.MapPost("news/like/remove", async (CreateNewsImpression.NewsLikeRemoveCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        });
    }
}