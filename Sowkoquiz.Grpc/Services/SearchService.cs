using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Sowkoquiz.Application.Search.SearchQuiz;
using Sowkoquiz.Application.Search.UserHistory;
using Enum = System.Enum;

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
                result.Quizzes.Select(quiz => new SearchQuizResponse.Types.Quiz
                {
                    Description = quiz.Description,
                    Id = quiz.Id!.Value,
                    Title = quiz.Title
                })
            },
            Total = result.TotalCount
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
                result.ActiveQuizzes.Select(q => new GetUserHistoryResponse.Types.HistoricalQuiz
                {
                    Id = q.Id!.Value,
                    Status = Enum.Parse<QuizStatus>(q.Status.ToString()),
                    Title = q.Definition.Title,
                    Date = Timestamp.FromDateTimeOffset(q.EndTime)
                })
            },
            Total = result.TotalCount
        };
    }
}