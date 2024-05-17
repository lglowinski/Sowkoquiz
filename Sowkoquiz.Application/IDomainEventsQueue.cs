using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Application;

public interface IDomainEventsQueue
{
    void Enqueue(IDomainEvent domainEvent);
    bool TryDequeue(out IDomainEvent domainEvent);
}