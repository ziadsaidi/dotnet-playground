
using Sales.Domain.Common;

namespace Sales.Domain.Entities
{
  public class Product : IEntity
  {
    public virtual Guid Id { get; set; } = Guid.CreateVersion7();
    public virtual required string Name { get; init; }

    public virtual double? Price { get; init; }

    public virtual ICollection<Sale> Sales { get; init; } = [];
  }
}
