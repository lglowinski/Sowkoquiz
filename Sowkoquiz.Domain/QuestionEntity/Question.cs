using System.Text.Json.Serialization;
using Sowkoquiz.Domain.Common;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Domain.QuestionEntity;

public class Question : Entity
{
    public Question(string text, List<Answer> answers, int id) : base(id)
    {
        Text = text;
        Answers = answers;
    }

    [JsonConstructor]
    public Question() : base(null)
    {
        
    }
    
    public string Text { get; init; } = null!;
    public virtual List<Answer> Answers { get; init; } = null!;
    protected override int HashCodeSeed => 32;
}