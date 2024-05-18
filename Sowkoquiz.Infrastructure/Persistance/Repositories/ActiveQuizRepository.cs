using System.Linq.Expressions;
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
            .Include(q => q.AnsweredQuestions)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task RefreshQuizAsync(int id, DateTime dateTime, CancellationToken cancellationToken = default)
    {
        await dbContext.ActiveQuizzes.Where(quiz => quiz.Id == id).ExecuteUpdateAsync(s =>
            s.SetProperty(quiz => quiz.EndTime, quiz => dateTime), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(ActiveQuiz quiz, CancellationToken cancellationToken = default)
    {
        dbContext.ActiveQuizzes.Update(quiz);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<(IEnumerable<ActiveQuiz> Quizzes, int TotalCount)> SearchAsync(string accessKey, Expression<Func<ActiveQuiz,object>>? orderByPredicate,
        string searchTerm = "", int take = 12, int skip = 0,
        CancellationToken cancellationToken = default)
    {
        var quizzes = dbContext
            .ActiveQuizzes
            .Where(quiz => quiz.AccessKey == accessKey)
            .Include(quiz => quiz.Definition)
            .AsQueryable();

        if (orderByPredicate is not null)
            quizzes = quizzes.OrderBy(orderByPredicate);
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            quizzes = quizzes
                .Where(quiz =>
                    EF.Functions.Like(quiz.Definition.Title, $"%{searchTerm}%")
                    || EF.Functions.Like(quiz.Definition.Description, $"%{searchTerm}%"));
        }

        var total = quizzes.Count();
        
        var result = await quizzes.Skip(skip).Take(take).ToListAsync(cancellationToken);

        return new ValueTuple<IEnumerable<ActiveQuiz>, int>(result, total);
    }

    public async Task<List<ActiveQuiz>> GetInactiveQuizzesAsync(int retentionTime, CancellationToken cancellationToken = default)
    {
        var retentionDate = DateTime.UtcNow.AddMinutes(-retentionTime);
        return await dbContext.ActiveQuizzes
            .Where(quiz => quiz.EndTime < retentionDate && quiz.Status == QuizStatus.Active)
            .ToListAsync(cancellationToken);
    }
}