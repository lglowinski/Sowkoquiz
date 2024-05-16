using Grpc.Core;
using MediatR;
using Sowkoquiz.Application.Search.SearchQuiz;
using Sowkoquiz.Application.Search.UserHistory;

namespace Sowkoquiz.Grpc.Services;

public class SearchService(ISender sender) : Sowkoquiz.Grpc.SearchService.SearchServiceBase
{
    public override async Task<SearchQuizResponse> SearchQuiz(SearchQuizRequest request, ServerCallContext context)
    {
        var result = await sender.Send(new SearchQuizQuery(request.Take, request.Skip, request.SearchTerm), context.CancellationToken);

        return new SearchQuizResponse
        {
            Quiz =
            {
                result.Select(quiz => new SearchQuizResponse.Types.Quiz
                {
                    Description = quiz.Description,
                    Id = quiz.Id!.Value,
                    Title = quiz.Title
                })
            }
        };
    }

    public override async Task<GetUserHistoryResponse> GetUserHistory(GetUserHistoryRequest request, ServerCallContext context)
    {
        var result =
            await sender.Send(
                new GetUserHistoryQuery(request.AccessKey, request.Take, request.Skip, request.SearchTerm),
                context.CancellationToken);

        return new GetUserHistoryResponse
        {
            HistoricalQuiz =
            {
                result.Select(q => new GetUserHistoryResponse.Types.HistoricalQuiz
                {
                    Id = q.Id!.Value,
                    Status = QuizStatus.Active,
                    Title = q.Definition.Title
                })
            }
        };
    }
}