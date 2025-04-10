

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
  public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
  {
    _ = builder.HasKey(x => x.Id)
     .HasName("pk_inventory_transaction");

    _ = builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

    _ = builder.Property(x => x.Timestamp)
            .HasColumnName("timestamp");

    _ = builder.Property(x => x.QuantityChange)
            .HasColumnName("quantity_change");

    _ = builder.Property(x => x.Reason)
            .HasConversion<string>()
             .HasColumnName("reason");

    _ = builder.HasOne(x => x.PerformedBy)
        .WithOne(x => x.InventoryTransaction)
        .HasForeignKey<InventoryTransaction>(it => it.PerformedByEmployeeId)
        .OnDelete(DeleteBehavior.SetNull)
        .HasConstraintName("fk_inventory_transactions_employee");

    builder.ToTable(TableNames.InventoryTransaction);
  }
}
