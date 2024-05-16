using MediatR;
using Sowkoquiz.Application.Common;

namespace Sowkoquiz.Application.Quiz.RandomQuiz;

public class RandomQuizQueryHandler(IQuizDefinitionRepository repository) : IRequestHandler<RandomQuizQuery, int>
{
    public async Task<int> Handle(RandomQuizQuery request, CancellationToken cancellationToken)
    {
        var quizDefCount = await repository.GetCountAsync(cancellationToken);
        
        var random = new Random();

        return random.Next(1, quizDefCount);
    }
}