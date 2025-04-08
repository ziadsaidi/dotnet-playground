using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class Customer : IEntity
{
  public Customer()
  { }
  private Customer(Guid id, string name)
  {
    Id = id;
    Name = name;
    Sales = [];
  }

  public virtual Guid Id { get; protected set; }
  public virtual string Name { get; protected set; } = null!;
  public virtual ICollection<Sale> Sales { get; protected init; } = [];

  public static Customer Create(string name)
  {
    return new Customer(Guid.CreateVersion7(), name);
  }

  public static Customer Create(Guid id, string name)
  {
    return new Customer(id, name);
  }

  public virtual void Update(string newName)
  {
    Name = newName;
  }
}
