using Sowkoquiz.Domain.QuestionEntity;

namespace Sowkoquiz.Domain.Extension;

internal static class QuestionPoolExtension
{
    internal static Question Random(this List<Question> questions, int seed = 420)
    {
        var random = new Random();
        
        var index = random.Next(questions.Count);
        
        return questions[index];
    }
}