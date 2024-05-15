using MediatR;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.ActiveQuizEntity.DomainEvents;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Application.Questions.QuestionAnswered;

public class QuestionAnsweredNotificationHandler(IActiveQuizRepository repository, IDateTimeProvider dateTimeProvider) : INotificationHandler<RefreshQuizEvent>
{
    public async Task Handle(RefreshQuizEvent notification, CancellationToken cancellationToken)
    {
        await repository.RefreshQuizAsync(notification.QuizId, dateTimeProvider.UtcNow, cancellationToken);
    }
}