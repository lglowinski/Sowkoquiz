using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Domain.ActiveQuizEntity.DomainEvents;

public record RefreshQuizEvent(int QuizId) : IDomainEvent
{
    public string EventName => nameof(RefreshQuizEvent);
}