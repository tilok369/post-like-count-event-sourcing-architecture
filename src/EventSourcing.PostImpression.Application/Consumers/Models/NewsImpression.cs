namespace EventSourcing.PostImpression.Application.Consumers.Models;

public record NewsImpression(string EventType, string EventData);