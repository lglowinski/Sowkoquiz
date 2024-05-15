using Grpc.Core;
using MediatR;
using Sowkoquiz.Application.Search.SearchQuiz;

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
}