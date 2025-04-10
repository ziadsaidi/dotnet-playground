
using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class Sale : IEntity, IAggregateRoot
{
  public virtual Guid Id { get; set; } = Guid.CreateVersion7();
  public virtual DateTime CreationDate { get; set; } = DateTime.UtcNow;

  public virtual Guid? CustomerId { get; set; }
  public virtual Customer? Customer { get; set; }

  public virtual Guid EmployeeId { get; set; }
  public virtual Employee Employee { get; set; } = null!;

  public virtual ICollection<SaleLineItem> LineItems { get; } = [];

  public virtual double TotalPrice => LineItems.Sum(i => i.TotalPrice);
}
