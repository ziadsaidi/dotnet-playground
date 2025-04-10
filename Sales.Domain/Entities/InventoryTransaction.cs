using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class InventoryTransaction : IEntity
{
  public virtual Guid Id { get; set; } = Guid.CreateVersion7();

  public virtual Guid ProductId { get; set; }
  public virtual Product Product { get; set; } = null!;

  public virtual DateTime Timestamp { get; set; } = DateTime.UtcNow;

  public virtual int QuantityChange { get; set; } // + = stock in, - = stock out

  public virtual Guid? PerformedByEmployeeId { get; set; }
  public virtual Employee? PerformedBy { get; set; }

  public virtual InventoryReason Reason { get; set; } = InventoryReason.ManualAdjustment;
}
