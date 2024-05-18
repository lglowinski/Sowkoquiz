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
    
    public float CorrectPercentage => (float)Correct / Max * 100;
    public float AnsweredPercentage => (float)Answered / Max * 100;

    public bool HasPassed(float threshold)
    {
        if (Math.Abs(AnsweredPercentage - 100) > 0.1)
            return false;
        
        return CorrectPercentage >= threshold;
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Answered;
        yield return Max;
    }
}