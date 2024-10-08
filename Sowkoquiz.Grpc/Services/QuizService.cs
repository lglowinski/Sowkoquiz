using ErrorOr;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Sowkoquiz.Application.Quiz.AnswerQuestion;
using Sowkoquiz.Application.Quiz.DeleteUserQuiz;
using Sowkoquiz.Application.Quiz.QuizDetails;
using Sowkoquiz.Application.Quiz.RandomQuiz;
using Sowkoquiz.Application.Quiz.StartQuiz;
using Sowkoquiz.Grpc.Mappers;

namespace Sowkoquiz.Grpc.Services;

public class QuizService(ISender sender) : Grpc.QuizService.QuizServiceBase
{
    public override async Task<AnswerQuestionResponse> AnswerQuestion(AnswerQuestionRequest request,
        ServerCallContext context)
    {
        var result = await sender.Send(
            new AnswerQuestionCommand(request.QuizId, request.QuestionId, request.Letter[0], request.AccessKey),
            context.CancellationToken);

        if (!result.IsError)
            return AnswerQuestionMapper.Map(result.Value);

        SetContextError(result.FirstError, ref context);
        throw new RpcException(new Status(StatusCode.NotFound, "Quiz not found"));
    }

    public override async Task<StartQuizResponse> StartQuiz(StartQuizRequest request, ServerCallContext context)
    {
        var result = await sender.Send(new StartQuizCommand(request.Id, request.AccessKey), context.CancellationToken);
        if (result.IsError)
        {
            SetContextError(result.FirstError, ref context);
            throw new RpcException(new Status(StatusCode.NotFound, "Quiz not found"));
        }

        var activeQuiz = result.Value;

        var firstQuestion = activeQuiz.GetQuestion();

        return new StartQuizResponse
        {
            Id = activeQuiz.Id!.Value,
            Question = new Question
            {
                Id = firstQuestion.Id!.Value,
                Answers =
                {
                    firstQuestion.Answers.Select(answer => new Question.Types.Answer
                        { Letter = answer.Letter.ToString(), Text = answer.Text })
                },
                Text = firstQuestion.Text
            }
        };
    }

    public override async Task<RandomQuizResponse> RandomQuiz(RandomQuizRequest request, ServerCallContext context)
    {
        var result = await sender.Send(new RandomQuizQuery(), context.CancellationToken);

        return new RandomQuizResponse
        {
            Id = result
        };
    }

    public override async Task<QuizDetailsResponse> GetQuizDetails(QuizDetailsRequest request,
        ServerCallContext context)
    {
        var result = await sender.Send(new QuizDetailsQuery(request.Id, request.AccessKey), 
            context.CancellationToken);

        if (result.IsError)
        {
            SetContextError(result.FirstError, ref context);
            throw new RpcException(new Status(StatusCode.NotFound, "Quiz not found"));
        }

        var dto = result.Value.Details;

        return new QuizDetailsResponse
        {
            Details = new QuizDetails
            {
                Name = dto.Name,
                Id = dto.Id,
                Progress = new QuizDetails.Types.Progress
                {
                    Correct = dto.ProgressDto.Correct,
                    Total = dto.ProgressDto.Total
                },
                Date = Timestamp.FromDateTimeOffset(dto.Date),
                AnsweredQuestion =
                {
                    dto.AnsweredQuestionDtos.Select(a => new QuizDetails.Types.AnsweredQuestion
                    {
                        Answer = a.Answer,
                        CorrectAnswer = a.Correct,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    })
                }
            }
        };
    }

    public override async Task<DeleteUserQuizResponse> DeleteUserQuiz(DeleteUserQuizRequest request, ServerCallContext context)
    {
        var result = await sender.Send(new DeleteUserQuizCommand(request.Id, request.AccessKey));

        if (!result)
            throw new RpcException(new Status(StatusCode.Unknown, "Failed to delete quiz"));

        return new DeleteUserQuizResponse(new DeleteUserQuizResponse
        {
            Success = result
        });
    }

    private static void SetContextError(Error error, ref ServerCallContext context)
    {
        context.ResponseTrailers.Add(error.Code, error.Description);
    }
}