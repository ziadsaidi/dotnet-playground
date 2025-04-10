using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public class SaleLineItemConfiguration : IEntityTypeConfiguration<SaleLineItem>
{
  public void Configure(EntityTypeBuilder<SaleLineItem> builder)
  {
    builder.HasKey(x => x.Id)
    .HasName("pk_products_saleline_item");

    builder.Property(x => x.Id)
           .ValueGeneratedNever()
           .HasColumnName("id");

    builder.Property(x => x.UnitPrice)
           .HasColumnName("unit_price");

    builder.Property(x => x.Quantity)
           .HasColumnName("quantity");
  }
}
