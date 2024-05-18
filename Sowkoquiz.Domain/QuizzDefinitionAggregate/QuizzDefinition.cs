using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sowkoquiz.Domain.ActiveQuizEntity;
using Sowkoquiz.Domain.Common;
using Sowkoquiz.Domain.QuestionEntity;

namespace Sowkoquiz.Domain.QuizzDefinitionAggregate;

public class QuizzDefinition(int? id = null) : Entity(id)
{
    public QuizzDefinition(string title, string description, int quizzSize, List<Question> questionPool, int id) : this(id)
    {
        Title = title;
        Description = description;
        QuizzSize = quizzSize;
        QuestionPool = questionPool;
    }

    [JsonConstructor]
    public QuizzDefinition() : this(null)
    {
        
    }
    
    [MaxLength(128)]
    public string Title { get; set; } = null!;
    [MaxLength(512)]
    public string Description { get; set; } = null!;
    public virtual List<Question> QuestionPool { get; set; } = null!;
    public int QuizzSize { get; set; }
    
    [Range(0, 100)]
    public float PassedThreshold { get; set; } = 70;
    
    protected override int HashCodeSeed => 11;

    public ActiveQuiz Start(string requestAccessKey, IDateTimeProvider dateTimeProvider)
    {
        return new ActiveQuiz(this, requestAccessKey, dateTimeProvider.UtcNow, new Progress(0, QuizzSize, 0));
    }
}