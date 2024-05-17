using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Application.Search.SearchQuiz;

public record SearchQuizQueryResponse(IEnumerable<QuizzDefinition> Quizzes , int TotalCount);