using System.Text.Json.Serialization;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Domain.ActiveQuizEntity;

public class Progress : ValueObject
{
    public Progress(int answered, int max, int correct)
    {
        Answered = answered;
        Max = max;
        Correct = correct;
    }

    [JsonConstructor]
    public Progress()
    {
        
    }

    public int Answered { get; set; }
    public int Max { get; init; }
    
    public int Correct { get; set; }
    
    public decimal Percentage => (decimal)Answered / Max * 100;
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Answered;
        yield return Max;
    }
}