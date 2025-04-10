
using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class Product : IEntity, IAggregateRoot
{
  public virtual Guid Id { get; set; } = Guid.CreateVersion7();

  public virtual required string Name { get; init; }

  public virtual double? Price { get; init; }

  public virtual int StockQuantity { get; set; } // Should be adjusted when selling/restocking

  public virtual ICollection<InventoryTransaction> InventoryTransactions { get; init; } = [];

  public virtual ICollection<SaleLineItem> SaleLineItems { get; init; } = [];
}
