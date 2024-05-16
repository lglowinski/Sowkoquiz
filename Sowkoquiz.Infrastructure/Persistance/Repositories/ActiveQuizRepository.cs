using Microsoft.EntityFrameworkCore;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Infrastructure.Persistance.Repositories;

public class ActiveQuizRepository(SowkoquizDbContext dbContext) : IActiveQuizRepository
{
    public async Task<ActiveQuiz> AddAsync(ActiveQuiz quiz, CancellationToken cancellationToken = default)
    {
        var newQuiz = (await dbContext.ActiveQuizzes.AddAsync(quiz, cancellationToken)).Entity;
        await dbContext.SaveChangesAsync(cancellationToken);
        return newQuiz;
    }

    public async Task<ActiveQuiz?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ActiveQuizzes
            .Include(q => q.Definition)
            .Include(q => q.Definition.QuestionPool)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task RefreshQuizAsync(int id, DateTime dateTime, CancellationToken cancellationToken = default)
    {
        await dbContext.ActiveQuizzes.Where(quiz => quiz.Id == id).ExecuteUpdateAsync(s =>
            s.SetProperty(quiz => quiz.EndTime, quiz => dateTime), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ActiveQuiz quiz, CancellationToken cancellationToken = default)
    {
        dbContext.ActiveQuizzes.Update(quiz);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActiveQuiz>> SearchAsync(string accessKey, string searchTerm = "", int take = 12, int skip = 0,
        CancellationToken cancellationToken = default)
    {
        var quizzes = dbContext
            .ActiveQuizzes
            .Where(quiz => quiz.AccessKey == accessKey)
            .Include(quiz => quiz.Definition)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            quizzes = quizzes
                .Where(quiz =>
                    EF.Functions.Like(quiz.Definition.Title, $"%{searchTerm}%")
                    || EF.Functions.Like(quiz.Definition.Description, $"%{searchTerm}%"));
        }

        return await quizzes.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }
}