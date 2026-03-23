using Kongroo.CloudGames.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kongroo.CloudGames.Identity.Infrastructure;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).HasConversion(id => id.Value, value => new UserId(value));

        builder.HasIndex(user => user.Username).IsUnique();
        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(user => user.Username).HasMaxLength(User.UsernameMaxLength);
        builder.Property(user => user.Email).HasMaxLength(User.EmailMaxLength);
        builder.Property(user => user.PasswordHash).HasMaxLength(User.PasswordHashMaxLength);
        builder.Property(user => user.SecurityStamp).HasMaxLength(User.SecurityStampLength).IsFixedLength();
        builder.Property(user => user.Name).HasMaxLength(User.NameMaxLength);
        builder
            .Property(user => user.Role)
            .HasConversion(role => role.Value, value => UserRole.From(value))
            .HasMaxLength(16);
    }
}
