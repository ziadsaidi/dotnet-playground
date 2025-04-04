namespace Persistance.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
  public void Configure(EntityTypeBuilder<Sale> builder)
  {
    builder.HasKey(e => e.Id)
           .HasName("sale_id");

    builder.Property(e => e.Id)
           .ValueGeneratedNever()
           .HasColumnName("id");

    builder.Property(e => e.UnitPrice)
           .IsRequired()
           .HasColumnName("unit_price");

    builder.Property(e => e.Quantity)
           .IsRequired()
           .HasColumnName("quantity");

    builder.Property(e => e.TotalPrice)
           .IsRequired()
           .HasColumnName("total_price");

    // Relationships 
    builder.HasOne(c => c.Customer)
           .WithMany(c => c.Sales)
           .HasForeignKey(c => c.CustomerId)  // Use the foreign key here
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("sales_customer_fkey");

    builder.HasOne(c => c.Employee)
           .WithMany(c => c.Sales)
           .HasForeignKey(c => c.EmployeeId)  // Use the foreign key here
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("sales_employee_fkey");

    builder.HasOne(c => c.Product)
           .WithMany(c => c.Sales)
           .HasForeignKey(c => c.ProductId)  // Use the foreign key here
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("sales_product_fkey");
  }
}
