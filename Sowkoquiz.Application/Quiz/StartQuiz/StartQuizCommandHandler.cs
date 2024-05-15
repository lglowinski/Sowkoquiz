using ErrorOr;
using MediatR;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.ActiveQuizEntity;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Application.Quiz.StartQuiz;

public class StartQuizCommandHandler(
    IQuizDefinitionRepository quizDefinitionRepository,
    IActiveQuizRepository activeQuizRepository,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<StartQuizCommand, ErrorOr<ActiveQuiz>>
{
    public async Task<ErrorOr<ActiveQuiz>> Handle(StartQuizCommand request, CancellationToken cancellationToken)
    {
        var definition = await quizDefinitionRepository.FindByIdAsync(request.QuizId, cancellationToken);

        if (definition is null)
            return Error.NotFound("Quiz not found");
        
        var newQuiz = definition.Start(request.AccessKey, dateTimeProvider);

        await activeQuizRepository.AddAsync(newQuiz, cancellationToken);

        return newQuiz;
    }
}