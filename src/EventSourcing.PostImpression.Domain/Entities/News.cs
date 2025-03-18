using EventSourcing.PostImpression.Domain.Events;

namespace EventSourcing.PostImpression.Domain.Entities;

public class News
{
    public Guid Id { get; private set; }
    
    private readonly HashSet<int> _likedByUserIds = [];
    private readonly List<NewsImpressionBaseEvent> _impressionsEvents = [];

    private News()
    {
    }
    
    public int TotalLikes => _likedByUserIds.Count;

    public static News Create(Guid id)
    {
        return new News() { Id = id };
    }

    public void Like(int likedByUserId)
    {
        if (_likedByUserIds.Contains(likedByUserId)) 
            return;
        Apply(new NewsLikedEvent{NewsId = Id, UserId = likedByUserId});
    }
    
    public void RemoveLike(int likedByUserId)
    {
        Apply(new NewsLikeRemovedEvent(){NewsId = Id, UserId = likedByUserId});
    }

    public void Apply(NewsImpressionBaseEvent @event)
    {
        When(@event);
        _impressionsEvents.Add(@event);
    }

    private void When(NewsImpressionBaseEvent @event)
    {
        switch (@event)
        {
            case NewsLikedEvent e:
                _likedByUserIds.Add(e.UserId);
                break;
            case NewsLikeRemovedEvent e:
                _likedByUserIds.Remove(e.UserId);
                break;
        }
    }
    
    public IReadOnlyCollection<NewsImpressionBaseEvent> GetEvents() => _impressionsEvents;
    public void ClearEvents() => _impressionsEvents.Clear();
}