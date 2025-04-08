using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    _ = builder.HasKey(static e => e.Id)
      .HasName("product_id");
    _ = builder.Property(static e => e.Id)
    .ValueGeneratedNever()
    .HasColumnName("id");
    _ = builder.Property(static e => e.Name)
    .IsRequired()
    .HasColumnName("name");
    _ = builder.ToTable(TableNames.Products);
  }
}
