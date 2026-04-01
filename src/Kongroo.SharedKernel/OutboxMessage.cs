using System.Text.Json;

namespace Kongroo.SharedKernel;

public sealed class OutboxMessage : Entity<OutboxMessageId>
{
    private OutboxMessage() { }

    public required DateTimeOffset OccurredAt { get; init; }

    public required Type Type { get; init; }

    public required string Payload { get; init; }

    public DateTimeOffset? ProcessedAt { get; private set; }

    public DateTimeOffset? FailedAt { get; private set; }

    public string? Error { get; private set; }

    public static OutboxMessage Create<T>(T domainEvent)
        where T : DomainEvent =>
        new()
        {
            Id = OutboxMessageId.Create(),
            OccurredAt = domainEvent.OccurredAt,
            Type = typeof(T),
            Payload = JsonSerializer.Serialize(domainEvent),
        };

    public DomainEvent GetDomainEvent()
    {
        if (!Type.IsAssignableTo(typeof(DomainEvent)))
        {
            throw new InvalidOperationException($"The type '{Type}' is not a domain event.");
        }

        var domainEvent =
            JsonSerializer.Deserialize(Payload, Type)
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
