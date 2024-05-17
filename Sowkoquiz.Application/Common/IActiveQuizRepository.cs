using System.Linq.Expressions;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Application.Common;

public interface IActiveQuizRepository
{
    public Task<ActiveQuiz> AddAsync(ActiveQuiz quiz, CancellationToken cancellationToken = default);
    public Task<ActiveQuiz?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task RefreshQuizAsync(int id, DateTime dateTime, CancellationToken cancellationToken = default);
    public Task UpdateAsync(ActiveQuiz quiz, CancellationToken cancellationToken = default);

    public Task<(IEnumerable<ActiveQuiz> Quizzes, int TotalCount)> SearchAsync(string accessKey,
        Expression<Func<ActiveQuiz,object>>? orderByPredicate, string searchTerm = "", int take = 12,
        int skip = 0, CancellationToken cancellationToken = default);
}