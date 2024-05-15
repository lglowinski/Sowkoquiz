using System.Collections.Concurrent;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Infrastructure.DomainEvents;

public class DomainEventsQueue : ConcurrentQueue<IDomainEvent>
{
    
}