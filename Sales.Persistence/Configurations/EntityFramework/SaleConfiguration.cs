namespace Sales.Persistence.Configurations.EntityFramework
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Sales.Domain.Entities;
  using Sales.Persistence.Constants;

  public class SaleConfiguration : IEntityTypeConfiguration<Sale>
  {
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
      _ = builder.HasKey(e => e.Id)
             .HasName("sale_id");

      _ = builder.Property(e => e.Id)
             .ValueGeneratedNever()
             .HasColumnName("id");

      _ = builder.Property(e => e.UnitPrice)
             .IsRequired()
             .HasColumnName("unit_price");

      _ = builder.Property(e => e.Quantity)
             .IsRequired()
             .HasColumnName("quantity");

      _ = builder.Property(e => e.TotalPrice)
             .IsRequired()
             .HasColumnName("total_price");

      // Relationships 
      _ = builder.HasOne(c => c.Customer)
             .WithMany(c => c.Sales)
             .HasForeignKey(c => c.CustomerId)  // Use the foreign key here
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("sales_customer_fkey");

      _ = builder.HasOne(c => c.Employee)
             .WithMany(c => c.Sales)
             .HasForeignKey(c => c.EmployeeId)  // Use the foreign key here
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("sales_employee_fkey");

      _ = builder.HasOne(c => c.Product)
             .WithMany(c => c.Sales)
             .HasForeignKey(c => c.ProductId)  // Use the foreign key here
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("sales_product_fkey");

      _ = builder.ToTable(TableNames.Sales);
    }
  }
}
