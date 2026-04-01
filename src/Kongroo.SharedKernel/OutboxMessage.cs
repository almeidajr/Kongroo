using System.Text.Json;

namespace Kongroo.SharedKernel;

public sealed class OutboxMessage : Entity<OutboxMessageId>
{
    private OutboxMessage() { }

    public required DateTimeOffset OccurredAt { get; init; }

    public required string EventType { get; init; }

    public required string Payload { get; init; }

    public DateTimeOffset? ProcessedAt { get; private set; }

    public DateTimeOffset? FailedAt { get; private set; }

    public string? Error { get; private set; }

    public static OutboxMessage Create<T>(T domainEvent)
        where T : DomainEvent
    {
        var eventType =
            typeof(T).AssemblyQualifiedName
            ?? throw new InvalidOperationException($"Unable to persist the type '{typeof(T)}'.");
        var payload = JsonSerializer.Serialize(domainEvent);

        return new OutboxMessage
        {
            Id = OutboxMessageId.Create(),
            OccurredAt = domainEvent.OccurredAt,
            EventType = eventType,
            Payload = payload,
        };
    }

    public DomainEvent GetDomainEvent()
    {
        var domainEventType =
            Type.GetType(EventType, throwOnError: true)
            ?? throw new InvalidOperationException($"The type '{EventType}' could not be resolved.");

        if (!domainEventType.IsAssignableTo(typeof(DomainEvent)))
        {
            throw new InvalidOperationException($"The type '{EventType}' is not a domain event.");
        }

        var domainEvent =
            JsonSerializer.Deserialize(Payload, domainEventType)
            ?? throw new InvalidOperationException("Unable to deserialize the outbox payload.");

        return (DomainEvent)domainEvent;
    }

    public void MarkProcessed(DateTimeOffset processedAt)
    {
        ProcessedAt = processedAt;

        FailedAt = null;
        Error = null;
    }

    public void MarkFailed(DateTimeOffset failedAt, string error)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(error);

        FailedAt = failedAt;
        Error = error;

        ProcessedAt = null;
    }
}
