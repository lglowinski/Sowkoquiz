using System.Text.Json.Serialization;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Domain.QuestionEntity;

public class Answer : ValueObject
{
    public Answer(char letter, string text, bool isCorrect)
    {
        Letter = letter;
        Text = text;
        IsCorrect = isCorrect;
    }
    
    [JsonConstructor]
    public Answer()
    {
        
    }
    
    public char Letter { get; init; }
    public string Text { get; init; } = null!;
    public bool IsCorrect { get; init; }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Text;
        yield return IsCorrect;
    }
}