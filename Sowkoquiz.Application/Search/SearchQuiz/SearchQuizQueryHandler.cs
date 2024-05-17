using MediatR;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Application.Search.SearchQuiz;

public class SearchQuizQueryHandler(IQuizDefinitionRepository quizDefinitionRepository)
    : IRequestHandler<SearchQuizQuery, SearchQuizQueryResponse>
{
    public async Task<SearchQuizQueryResponse> Handle(SearchQuizQuery request,
        CancellationToken cancellationToken)
    {
        var result = await quizDefinitionRepository.SearchAsync(request.Take, request.Skip, request.SearchTerm, cancellationToken);

        return new SearchQuizQueryResponse(result.Quizzes, result.Total);
    }
}