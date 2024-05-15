using Sowkoquiz.Application.Quiz.AnswerQuestion;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Grpc.Mappers;

public static class AnswerQuestionMapper
{
    public static AnswerQuestionResponse Map(AnswerQuestionCommandResult result)
    {
        result.ValidateState();

        return result.CurrentQuestion is null ? GetScore(result.Progress!) : NextQuestion(result.CurrentQuestion);
    }

    private static AnswerQuestionResponse GetScore(Progress activeQuizProgress)
    {
        return new AnswerQuestionResponse
        {
            Score = new Score
            {
                Correct = activeQuizProgress.Correct,
                Total = activeQuizProgress.Max,
            }
        };
    }

    private static AnswerQuestionResponse NextQuestion(Domain.QuestionEntity.Question nextQuestion)
    {
        return new AnswerQuestionResponse
        {
            NextQuestion = new Question
            {
                Id = nextQuestion.Id!.Value,
                Answers =
                {
                    nextQuestion.Answers.Select(answer => new Question.Types.Answer
                        { Letter = answer.Letter.ToString(), Text = answer.Text.ToString() })
                },
                Text = nextQuestion.Text
            }
        };
    }
}