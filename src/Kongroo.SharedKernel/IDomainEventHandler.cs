namespace Kongroo.SharedKernel;

public interface IDomainEventHandler
{
    Type EventType { get; }

    Task HandleAsync(DomainEvent domainEvent, CancellationToken cancellationToken);
}
