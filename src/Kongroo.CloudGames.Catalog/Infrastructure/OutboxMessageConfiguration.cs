using Kongroo.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kongroo.CloudGames.Catalog.Infrastructure;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(message => message.Id);
        builder.Property(message => message.Id).HasConversion(id => id.Value, value => OutboxMessageId.From(value));

        builder.Property(message => message.OccurredAt).HasPrecision(0);
        builder.Property(message => message.EventType);
        builder.Property(message => message.Payload);
        builder.Property(message => message.ProcessedAt).HasPrecision(0);
        builder.Property(message => message.FailedAt).HasPrecision(0);
    }
}
