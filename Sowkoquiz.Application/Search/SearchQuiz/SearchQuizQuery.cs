using MediatR;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Application.Search.SearchQuiz;

public record SearchQuizQuery
    : IRequest<IEnumerable<QuizzDefinition>>
{
    public int Take { get; set; }

    public int Skip { get; set; }

    public string SearchTerm { get; set; }

    public SearchQuizQuery(int? take, int? skip, string? searchTerm)
    {
        Take = take ?? 12;
        Skip = skip ?? 0;
        SearchTerm = searchTerm ?? string.Empty;
    }
}