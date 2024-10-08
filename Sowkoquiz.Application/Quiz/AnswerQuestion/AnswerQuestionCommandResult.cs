using Sowkoquiz.Domain.ActiveQuizEntity;
using Sowkoquiz.Domain.QuestionEntity;

namespace Sowkoquiz.Application.Quiz.AnswerQuestion;

public record AnswerQuestionCommandResult
{
    public Question? CurrentQuestion { get; init; }
    public Progress? Progress { get; init; }
    public float PassThreshold { get; init; } 

    public AnswerQuestionCommandResult(Question question)
    {
        CurrentQuestion = question;
    }

    public AnswerQuestionCommandResult(Progress progress, float passThreshold)
    {
        Progress = progress;
        PassThreshold = passThreshold;
    }

    public void ValidateState()
    {
        if (CurrentQuestion is null && Progress is null)
            throw new InvalidOperationException("Both CurrentQuestion and Progress cannot be null");
    }
}