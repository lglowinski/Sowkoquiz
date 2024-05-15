using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Application.Common;

public interface IActiveQuizRepository
{
    public Task<ActiveQuiz> AddAsync(ActiveQuiz quiz, CancellationToken cancellationToken = default);
    public Task<ActiveQuiz?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task RefreshQuizAsync(int id, DateTime dateTime, CancellationToken cancellationToken = default);
    public Task UpdateAsync(ActiveQuiz quiz, CancellationToken cancellationToken);
}