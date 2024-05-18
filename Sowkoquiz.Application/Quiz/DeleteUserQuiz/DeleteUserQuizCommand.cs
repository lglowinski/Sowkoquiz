using MediatR;

namespace Sowkoquiz.Application.Quiz.DeleteUserQuiz;

public record DeleteUserQuizCommand(int Id, string AccessKey) : IRequest<bool>;