using MediatR;
using Sowkoquiz.Application.Common;

namespace Sowkoquiz.Application.Search.UserHistory;

public class GetUserHistoryQueryHandler(IActiveQuizRepository activeQuizRepository)
    : IRequestHandler<GetUserHistoryQuery, GetUserHistoryQueryResponse>
{
    public async Task<GetUserHistoryQueryResponse> Handle(GetUserHistoryQuery query, CancellationToken cancellationToken)
    {
        var activeQuizzes = await activeQuizRepository.SearchAsync(query.AccessKey, 
            a => a.EndTime, query.SearchTerm, take: query.Take,
            skip: query.Skip, cancellationToken: cancellationToken);

        return new GetUserHistoryQueryResponse(activeQuizzes.Quizzes, activeQuizzes.TotalCount);
    }
}