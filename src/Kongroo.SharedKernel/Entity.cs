namespace Kongroo.SharedKernel;

public abstract class Entity<TId>(TId id)
    where TId : IEquatable<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TId Id { get; protected init; } = id;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
