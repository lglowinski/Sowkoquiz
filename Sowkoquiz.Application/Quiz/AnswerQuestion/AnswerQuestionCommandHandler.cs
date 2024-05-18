using ErrorOr;
using MediatR;
using Sowkoquiz.Application.Common;

namespace Sowkoquiz.Application.Quiz.AnswerQuestion;

public class AnswerQuestionCommandHandler(
    IActiveQuizRepository activeQuizRepository,
    IDomainEventsQueue domainEventsQueue)
    : IRequestHandler<AnswerQuestionCommand, ErrorOr<AnswerQuestionCommandResult>>
{
    public async Task<ErrorOr<AnswerQuestionCommandResult>> Handle(AnswerQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var quiz = await activeQuizRepository.FindByIdAsync(request.ActiveQuizId, cancellationToken);

        if (quiz is null)
            return Error.NotFound("Quiz not found");

        var result = quiz.AnswerQuestion(request.QuestionId, request.Letter, request.AccessKey);

        await activeQuizRepository.UpdateAsync(quiz, cancellationToken);

        return result.Value is null
            ? new AnswerQuestionCommandResult(quiz.Progress, quiz.Definition.PassedThreshold)
            : new AnswerQuestionCommandResult(result.Value);
    }
}