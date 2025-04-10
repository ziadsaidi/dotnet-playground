using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    _ = builder.HasKey(b => b.Id)
      .HasName("pk_users");

    _ = builder.Property(b => b.Id)
            .IsRequired()
            .HasColumnName("id")
            .ValueGeneratedNever();

    _ = builder.Property(b => b.Email)
           .IsRequired()
           .HasMaxLength(100)
           .HasColumnName("email");

    _ = builder.Property(b => b.Username)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("username");

    _ = builder.Property(b => b.PasswordHash)
          .HasColumnName("password_hash");

    _ = builder.Property(b => b.Role)
                .HasColumnName("role")
                .HasConversion<string>();

    _ = builder.HasIndex(b => b.Email)
            .HasDatabaseName("IX_Users_Email")
            .IsUnique();
    _ = builder.HasIndex(b => b.Username)
            .HasDatabaseName("IX_Users_Username")
            .IsUnique();



    builder.ToTable(TableNames.Users);
  }
}
