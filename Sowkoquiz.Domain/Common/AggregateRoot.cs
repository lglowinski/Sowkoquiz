namespace Sowkoquiz.Domain.Common;

public abstract class AggregateRoot(int? id) : Entity(id)
{
    protected readonly List<IDomainEvent> DomainEvents = [];

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = DomainEvents.ToList();
        DomainEvents.Clear();

        return copy;
    }
}