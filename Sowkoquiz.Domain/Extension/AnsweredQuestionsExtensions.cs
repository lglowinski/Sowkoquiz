using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Domain.Extension;

public static class AnsweredQuestionsExtensions
{
    public static bool AlreadyAnswered(this List<AnsweredQuestion> answeredQuestions, int questionId)
        => answeredQuestions.Exists(x => x.Id == questionId);
}