namespace EventSourcing.PostImpression.Domain.Events;

public class NewsImpressionBaseEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid NewsId { get; init; }
    public int UserId { get; init; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}