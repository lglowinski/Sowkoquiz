using ErrorOr;
using MediatR;

namespace Sowkoquiz.Application.Quiz.QuizDetails;

public record QuizDetailsQuery(int Id, string AccessKey) : IRequest<ErrorOr<QuizDetailsQueryResponse>>;