using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Domain.ActiveQuizEntity.DomainEvents;

public record QuizFinishedEvent(int QuizId) : IDomainEvent
{
    public string EventName => nameof(QuizFinishedEvent);
}