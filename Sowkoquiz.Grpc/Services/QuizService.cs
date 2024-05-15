using ErrorOr;
using Grpc.Core;
using MediatR;
using Sowkoquiz.Application.Quiz.AnswerQuestion;
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
        return new AnswerQuestionResponse();

    }

    public override async Task<StartQuizResponse> StartQuiz(StartQuizRequest request, ServerCallContext context)
    {
        var result = await sender.Send(new StartQuizCommand(request.Id, request.AccessKey), context.CancellationToken);
        if (result.IsError)
        {
            SetContextError(result.FirstError, ref context);
            return new StartQuizResponse();
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

    private static void SetContextError(Error error, ref ServerCallContext context)
    {
        context.ResponseTrailers.Add(error.Code, error.Description);
    }
}