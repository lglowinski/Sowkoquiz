using MediatR;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.ActiveQuizEntity;
using Sowkoquiz.Domain.ActiveQuizEntity.DomainEvents;

namespace Sowkoquiz.Application.Quiz.QuizFinished;

public class QuizFinishedEventHandler(IActiveQuizRepository repository) : INotificationHandler<QuizFinishedEvent>
{
    public async Task Handle(QuizFinishedEvent notification, CancellationToken cancellationToken)
    {
        var updatedQuiz = notification.Quiz;
        
        updatedQuiz.Status = QuizStatus.Finished;

        await repository.UpdateAsync(updatedQuiz, cancellationToken);
    }
}