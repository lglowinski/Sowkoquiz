using Microsoft.EntityFrameworkCore;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Infrastructure.Persistance.Repositories;

public class QuizDefinitionRepository(SowkoquizDbContext dbContext) : IQuizDefinitionRepository
{
    public async Task<QuizzDefinition?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .QuizzDefinitions
            .Include(p => p.QuestionPool)
            .FirstOrDefaultAsync(quiz => quiz.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<QuizzDefinition>> SearchAsync(int take = 12, int skip = 0, string searchTerm = "",
        CancellationToken cancellationToken = default)
    {
        var quizzes = dbContext.QuizzDefinitions.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            quizzes = quizzes
                .Where(quiz =>
                    EF.Functions.Like(quiz.Description, $"%{searchTerm}%")
                    || EF.Functions.Like(quiz.Title, $"%{searchTerm}%"));
        }

        return await quizzes.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        var count = dbContext.QuizzDefinitions.Count();
        
        return Task.FromResult(count);
    }
}