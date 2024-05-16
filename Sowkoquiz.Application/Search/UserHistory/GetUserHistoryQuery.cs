using MediatR;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Application.Search.UserHistory;

public record GetUserHistoryQuery : IRequest<IEnumerable<ActiveQuiz>>
{
    public string AccessKey { get; }
    public int Take { get; }

    public int Skip { get; }

    public string SearchTerm { get; }

    public GetUserHistoryQuery(string accessKey, int? take, int? skip, string? searchTerm)
    {
        AccessKey = accessKey;
        Take = take ?? 12;
        Skip = skip ?? 0;
        SearchTerm = searchTerm ?? string.Empty;
    }
}