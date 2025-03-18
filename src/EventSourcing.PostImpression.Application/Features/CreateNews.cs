using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventSourcing.PostImpression.Application.Features;

public static class CreateNews
{
    public record CreateNewsCommand(Guid Id, string Title):IRequest<CreateNewsResponse>;
    public record CreateNewsResponse(Guid Id, string Title, DateTime CreatedAt);
    
    public class CreateNewsCommandHandler: IRequestHandler<CreateNewsCommand, CreateNewsResponse>
    {
        public Task<CreateNewsResponse> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CreateNewsResponse(request.Id, request.Title, DateTime.UtcNow));
        }
    }
}

public class CreateNewsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("news", async (CreateNews.CreateNewsCommand command, ISender sender) =>
        {
            var response = await sender.Send(command);
            return Results.Ok(response);
        });
    }
}