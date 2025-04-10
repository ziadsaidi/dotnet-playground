using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    _ = builder.HasKey(e => e.Id)
            .HasName("customer_id");

    _ = builder.Property(static e => e.Id)
       .ValueGeneratedNever()
       .HasColumnName("id");

    _ = builder.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");

    _ = builder.HasOne(e => e.User)
          .WithOne(c => c.Customer)
          .HasForeignKey<Customer>(e => e.UserId)
          .IsRequired(false)
          .OnDelete(DeleteBehavior.SetNull)
          .HasConstraintName("fk_Users_customers");

    _ = builder.ToTable(TableNames.Customers);
  }
}
