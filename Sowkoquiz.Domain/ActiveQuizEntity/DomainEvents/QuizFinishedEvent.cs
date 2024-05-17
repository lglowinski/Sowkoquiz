using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Domain.ActiveQuizEntity.DomainEvents;

public record QuizFinishedEvent(ActiveQuiz Quiz) : IDomainEvent
{
    public string EventName => nameof(QuizFinishedEvent);
}