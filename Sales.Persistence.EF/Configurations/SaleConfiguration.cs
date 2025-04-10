using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
  public void Configure(EntityTypeBuilder<Sale> builder)
  {
    _ = builder.HasKey(e => e.Id)
           .HasName("sale_id");

    _ = builder.Property(e => e.Id)
           .ValueGeneratedNever()
           .HasColumnName("id");

    _ = builder.Property(e => e.CreationDate)
           .HasColumnName("creation_date");

    // Relationships 
    _ = builder.HasOne(c => c.Customer)
           .WithMany(c => c.Sales)
           .HasForeignKey(c => c.CustomerId)
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("sales_customer_fkey");

    _ = builder.HasOne(c => c.Employee)
           .WithMany(c => c.Sales)
           .HasForeignKey(c => c.EmployeeId)
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("sales_employee_fkey");

    _ = builder.ToTable(TableNames.Sales);
  }
}
