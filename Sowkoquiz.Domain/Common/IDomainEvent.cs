using MediatR;

namespace Sowkoquiz.Domain.Common;

public interface IDomainEvent : INotification
{
    public string EventName { get; }
}