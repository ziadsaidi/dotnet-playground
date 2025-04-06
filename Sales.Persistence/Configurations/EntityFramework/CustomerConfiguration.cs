using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Configurations.EntityFramework;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    _ = builder.HasKey(e => e.Id)
            .HasName("customer_id");
    _ = builder.Property(e => e.Id)
       .ValueGeneratedNever()
       .HasColumnName("id");
    _ = builder.Property(e => e.Name)
         .IsRequired()
         .HasColumnName("name");
    _ = builder.ToTable(TableNames.Customers);
  }
}
