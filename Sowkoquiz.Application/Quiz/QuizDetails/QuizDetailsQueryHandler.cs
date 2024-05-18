using ErrorOr;
using MediatR;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Application.Dto;
using Sowkoquiz.Domain.ActiveQuizEntity;
using Sowkoquiz.Domain.QuestionEntity;

namespace Sowkoquiz.Application.Quiz.QuizDetails;

public class QuizDetailsQueryHandler(IActiveQuizRepository activeQuizRepository)
    : IRequestHandler<QuizDetailsQuery, ErrorOr<QuizDetailsQueryResponse>>
{
    public async Task<ErrorOr<QuizDetailsQueryResponse>> Handle(QuizDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var activeQuiz = await activeQuizRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (activeQuiz == null)
            return Error.NotFound();

        if (!activeQuiz.HasAccess(request.AccessKey))
            return Error.Forbidden("You can't access this quiz");

        var answered = MapAnswered(activeQuiz.AnsweredQuestions, activeQuiz.Definition.QuestionPool);
        var progress = MapProgress(activeQuiz.Progress);


        return new QuizDetailsQueryResponse(new QuizDetailsDto(activeQuiz.Id!.Value, activeQuiz.Definition.Title,
            answered, progress, activeQuiz.EndTime));
    }

    private static List<AnsweredQuestionDto> MapAnswered(IEnumerable<AnsweredQuestion> answeredQuestions,
        IEnumerable<Question> questionPool)
    {
        return answeredQuestions.Join(questionPool, aq => aq.Id,
            definitionQuestionPool => definitionQuestionPool.Id,
            (answered, question) =>
            {
                var userAnswer = question.Answers.First(a => a.Letter == answered.Letter);
                var correct = question.Answers.First(a => a.IsCorrect).Text;

                return new AnsweredQuestionDto(answered.Id, question.Text, userAnswer.Text, correct,
                    userAnswer.IsCorrect);
            }).ToList();
    }


    private static ProgressDto MapProgress(Progress activeQuizProgress)
        => new(activeQuizProgress.Correct, activeQuizProgress.Max);
}