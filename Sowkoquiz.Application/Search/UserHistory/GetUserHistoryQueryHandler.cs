using MediatR;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Application.Search.UserHistory;

public class GetUserHistoryQueryHandler(IActiveQuizRepository activeQuizRepository) : IRequestHandler<GetUserHistoryQuery, IEnumerable<ActiveQuiz>>
{
    public async Task<IEnumerable<ActiveQuiz>> Handle(GetUserHistoryQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}