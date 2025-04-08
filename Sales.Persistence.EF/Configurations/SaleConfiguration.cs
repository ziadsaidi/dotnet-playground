using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
  public void Configure(EntityTypeBuilder<Sale> builder)
  {
    _ = builder.HasKey(static e => e.Id)
           .HasName("sale_id");

    _ = builder.Property(static e => e.Id)
           .ValueGeneratedNever()
           .HasColumnName("id");

    _ = builder.Property(static e => e.UnitPrice)
           .IsRequired()
           .HasColumnName("unit_price");

    _ = builder.Property(static e => e.Quantity)
           .IsRequired()
           .HasColumnName("quantity");

    _ = builder.Property(static e => e.TotalPrice)
           .IsRequired()
           .HasColumnName("total_price");

    // Relationships 
    _ = builder.HasOne(static c => c.Customer)
           .WithMany(static c => c.Sales)
           .HasForeignKey(static c => c.CustomerId)  // Use the foreign key here
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("sales_customer_fkey");

    _ = builder.HasOne(static c => c.Employee)
           .WithMany(static c => c.Sales)
           .HasForeignKey(static c => c.EmployeeId)  // Use the foreign key here
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("sales_employee_fkey");

    _ = builder.HasOne(static c => c.Product)
           .WithMany(static c => c.Sales)
           .HasForeignKey(static c => c.ProductId)  // Use the foreign key here
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("sales_product_fkey");
    _ = builder.ToTable(TableNames.Sales);
  }
}
