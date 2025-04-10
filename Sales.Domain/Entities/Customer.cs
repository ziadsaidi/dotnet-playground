using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class Customer : IEntity, IAggregateRoot
{
  public virtual Guid Id { get; protected set; } = Guid.CreateVersion7();
  public virtual string Address { get; protected set; } = null!;
  public virtual Guid UserId { get; protected set; }
  public virtual User User { get; protected set; } = null!;
  public virtual ICollection<Sale> Sales { get; protected init; } = [];

  public static Customer Create(Guid userId, string address)
  {
    return new Customer
    {
      Id = Guid.CreateVersion7(),
      UserId = userId,
      Address = address
    };
  }

  public virtual void UpdateAddress(string newAddress)
  {
    Address = newAddress;
  }
}
