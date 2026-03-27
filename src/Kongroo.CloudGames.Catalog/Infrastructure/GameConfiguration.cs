using Kongroo.CloudGames.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kongroo.CloudGames.Catalog.Infrastructure;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(game => game.Id);
        builder.Property(game => game.Id).HasConversion(id => id.Value, value => GameId.From(value));

        builder
            .Property(game => game.Title)
            .HasConversion(title => title.Value, value => GameTitle.From(value))
            .HasMaxLength(GameTitle.MaxLength);
        builder
            .Property(game => game.Description)
            .HasConversion(description => description.Value, value => GameDescription.From(value))
            .HasMaxLength(GameDescription.MaxLength);
        builder.Property(game => game.Status).HasConversion<string>().HasMaxLength(16);

        builder.ComplexProperty(
            game => game.Price,
            moneyBuilder =>
            {
                moneyBuilder.Property(money => money.Amount).HasPrecision(18, 2);

                moneyBuilder
                    .Property(money => money.Currency)
                    .HasConversion(
                        currency => CurrencyMappings.ToCode(currency),
                        code => CurrencyMappings.FromCode(code)
                    )
                    .HasMaxLength(CurrencyMappings.Length)
                    .IsFixedLength();
            }
        );
    }
}
