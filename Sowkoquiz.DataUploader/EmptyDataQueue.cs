using Sowkoquiz.Application;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.DataUploader;

public class EmptyDataQueue : IDomainEventsQueue
{
    public void Enqueue(IDomainEvent domainEvent)
    {
        throw new NotImplementedException();
    }

    public bool TryDequeue(out IDomainEvent domainEvent)
    {
        throw new NotImplementedException();
    }
}