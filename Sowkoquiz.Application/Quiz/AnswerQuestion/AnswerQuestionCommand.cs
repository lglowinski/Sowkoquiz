using ErrorOr;
using MediatR;

namespace Sowkoquiz.Application.Quiz.AnswerQuestion;

public record AnswerQuestionCommand(int ActiveQuizId, int QuestionId, char Letter, string AccessKey)
    : IRequest<ErrorOr<AnswerQuestionCommandResult>>;