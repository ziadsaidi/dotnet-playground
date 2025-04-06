
using Sales.Domain.Common;

namespace Sales.Domain.Entities
{
  public class Employee : IEntity
  {
    public virtual Guid Id { get; set; } = Guid.NewGuid();

    public virtual required string Name { get; init; }

    public virtual ICollection<Sale> Sales { get; init; } = [];
  }
}
