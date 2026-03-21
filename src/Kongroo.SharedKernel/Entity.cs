namespace Kongroo.SharedKernel;

public abstract class Entity<TEntityId>
    where TEntityId : IEquatable<TEntityId>
{
    private readonly List<DomainEvent> _domainEvents = [];

    public required TEntityId Id { get; init; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
