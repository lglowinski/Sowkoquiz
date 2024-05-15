using MediatR;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Application.Search.SearchQuiz;

public class SearchQuizQueryHandler(IQuizDefinitionRepository quizDefinitionRepository)
    : IRequestHandler<SearchQuizQuery, IEnumerable<QuizzDefinition>>
{
    public async Task<IEnumerable<QuizzDefinition>> Handle(SearchQuizQuery request, CancellationToken cancellationToken)
        => await quizDefinitionRepository.SearchAsync(request.Take, request.Skip, request.SearchTerm, cancellationToken);
}