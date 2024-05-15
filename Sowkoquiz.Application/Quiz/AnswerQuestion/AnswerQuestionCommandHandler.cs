using ErrorOr;
using MediatR;
using Sowkoquiz.Application.Common;

namespace Sowkoquiz.Application.Quiz.AnswerQuestion;

public class AnswerQuestionCommandHandler(IActiveQuizRepository activeQuizRepository)
    : IRequestHandler<AnswerQuestionCommand, ErrorOr<AnswerQuestionCommandResult>>
{
    public async Task<ErrorOr<AnswerQuestionCommandResult>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await activeQuizRepository.FindByIdAsync(request.ActiveQuizId, cancellationToken);

        if (quiz is null)
            return Error.NotFound("Quiz not found");
        
        var result = quiz.AnswerQuestion(request.QuestionId, request.Letter, request.AccessKey);

        if (result.Value is null)
            return new AnswerQuestionCommandResult(quiz.Progress);
        
        await activeQuizRepository.UpdateAsync(quiz, cancellationToken);

        return new AnswerQuestionCommandResult(result.Value);
    }
}