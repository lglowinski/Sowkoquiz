using System.Text.Json.Serialization;
using ErrorOr;
using Sowkoquiz.Domain.ActiveQuizEntity.DomainEvents;
using Sowkoquiz.Domain.Common;
using Sowkoquiz.Domain.Extension;
using Sowkoquiz.Domain.QuestionEntity;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Domain.ActiveQuizEntity;

public class ActiveQuiz : AggregateRoot
{
    public ActiveQuiz(QuizzDefinition definition,
        List<int?> answered, 
        DateTime endTime,
        string accessKey, 
        Progress progress, int id = -7) : this(definition, accessKey, endTime, progress, id)
    {
        AnsweredQuestionsId = answered;
    }

    [JsonConstructor]
    public ActiveQuiz() : base(null)
    {
        
    }

    public ActiveQuiz(QuizzDefinition definition, string accessKey, DateTime endTime, Progress progress
        ,int? id = null) : base(id)
    {
        Definition = definition;
        AccessKey = accessKey;
        EndTime = endTime;
        Progress = progress;
    }
    
    public virtual QuizzDefinition Definition { get; init; }
    public Progress Progress { get; set; } = new();
    public bool IsFinished => Progress.Percentage == 100;
    public List<int?> AnsweredQuestionsId { get; init; } = [];
    public DateTime EndTime { get; init; }
    public string AccessKey { get; init; }
    
    protected override int HashCodeSeed => 7;
    
    public ErrorOr<Question?> AnswerQuestion(int questionId, char letter, string accessKey)
    {
        if (!accessKey.Equals(AccessKey))
            return Error.Forbidden(description: "Access is not allowed for this quiz");
        
        var question = Definition.QuestionPool.First(q => q.Id == questionId);
        var answer = question.Answers.First(a => a.Letter == letter);

        Progress.Answered++;
        AnsweredQuestionsId.Add(questionId);
        
        if (answer.IsCorrect)
            Progress.Correct++;

        if (Progress.Percentage == 100)
        {
            DomainEvents.Add(new QuizFinishedEvent(Id!.Value));
            return null as Question;
        }
            
        
        DomainEvents.Add(new RefreshQuizEvent(Id!.Value));

        return GetQuestion();
    }
    
    public Question GetQuestion()
    {        
        DomainEvents.Add(new RefreshQuizEvent(Id!.Value));
        while (true)
        {
            var question = Definition.QuestionPool.Random();

            if (AnsweredQuestionsId.Contains(question.Id))
                continue;
            
            return question;
        }
    }
}