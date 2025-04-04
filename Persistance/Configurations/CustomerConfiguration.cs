using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    builder.HasKey(e => e.Id)
            .HasName("customer_id");
    builder.Property(e => e.Id)
       .ValueGeneratedNever()
       .HasColumnName("id");

    builder.Property(e => e.Name)
         .IsRequired()
         .HasColumnName("name");

    builder.ToTable("customers");
  }
}
