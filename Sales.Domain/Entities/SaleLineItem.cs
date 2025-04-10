using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class SaleLineItem : IEntity
{
  public virtual Guid Id { get; set; } = Guid.CreateVersion7();

  public virtual Guid SaleId { get; set; }
  public virtual Sale Sale { get; set; } = null!;

  public virtual Guid ProductId { get; set; }
  public virtual Product Product { get; set; } = null!;

  public virtual double UnitPrice { get; set; }

  public virtual int Quantity { get; set; }

  public virtual double TotalPrice => UnitPrice * Quantity;
}
