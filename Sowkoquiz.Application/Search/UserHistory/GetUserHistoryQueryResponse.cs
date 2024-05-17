using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Application.Search.UserHistory;

public record GetUserHistoryQueryResponse(IEnumerable<ActiveQuiz> ActiveQuizzes, int TotalCount);