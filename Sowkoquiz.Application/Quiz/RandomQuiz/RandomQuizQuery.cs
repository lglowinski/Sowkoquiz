using MediatR;

namespace Sowkoquiz.Application.Quiz.RandomQuiz;

public record RandomQuizQuery() : IRequest<int>;