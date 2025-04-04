using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.HasKey(e => e.Id)
      .HasName("product_id");

    builder.Property(e => e.Id)
    .ValueGeneratedNever()
    .HasColumnName("id");

    builder.Property(e => e.Name)
    .IsRequired()
    .HasColumnName("name");

    builder.ToTable("products");
  }
}
