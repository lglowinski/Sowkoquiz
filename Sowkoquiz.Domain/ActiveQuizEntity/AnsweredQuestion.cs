using System.Text.Json.Serialization;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Domain.ActiveQuizEntity;

public class AnsweredQuestion : ValueObject
{
    [JsonConstructor]
    public AnsweredQuestion()
    {
        
    }
    
    public AnsweredQuestion(int questionId, char letter)
    {
        Id = questionId;
        Letter = letter;
    }

    public int Id { get; set; }
    public char Letter { get; set; }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Letter;
    }
}