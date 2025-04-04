using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Configurations.EntityFramework
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      _ = builder.HasKey(e => e.Id)
        .HasName("product_id");

      _ = builder.Property(e => e.Id)
      .ValueGeneratedNever()
      .HasColumnName("id");

      _ = builder.Property(e => e.Name)
      .IsRequired()
      .HasColumnName("name");

      _ = builder.ToTable(TableNames.Products);
    }
  }
}
