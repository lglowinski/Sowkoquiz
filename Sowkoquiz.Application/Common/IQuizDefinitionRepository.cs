using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Application.Common;

public interface IQuizDefinitionRepository
{
    public Task<QuizzDefinition?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<QuizzDefinition>> SearchAsync(int take = 12, int skip = 0, string searchTerm = "", CancellationToken cancellationToken = default);
}