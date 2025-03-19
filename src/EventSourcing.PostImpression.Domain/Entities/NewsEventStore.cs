namespace EventSourcing.PostImpression.Domain.Entities;

public class NewsEventStore
{
    public Guid Id { get; set; }
    public Guid NewsId { get; set; }
    public required string EventType { get; set; }
    public required string EventData { get; set; }
    public DateTime OccurredOn { get; set; }
    public bool Processed { get; set; } = false;
    public DateTime? ProcessedOn { get; set; }
}