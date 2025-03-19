namespace EventSourcing.PostImpression.Domain.Entities;

public class NewsImpressionSummary
{
    public Guid NewsId { get; set; }
    public int TotalLikes { get; set; } = 0;
    public DateTime UpdatedOn { get; set; }
}