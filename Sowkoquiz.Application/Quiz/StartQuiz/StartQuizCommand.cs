using ErrorOr;
using MediatR;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Application.Quiz.StartQuiz;

public record StartQuizCommand(int QuizId, string AccessKey) : IRequest<ErrorOr<ActiveQuiz>>;