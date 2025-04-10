using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class Employee : IEntity, IAggregateRoot
{
  public virtual Guid Id { get; set; } = Guid.CreateVersion7();
  public virtual EmployeePosition Position { get; set; }
  public virtual double Salary { get; set; }
  public virtual Guid UserId { get; set; }
  public virtual User User { get; set; } = null!;
  public virtual ICollection<Sale> Sales { get; init; } = [];

  public InventoryTransaction InventoryTransaction { get; init; } = null!;

  public static Employee Create(Guid userId, EmployeePosition position, double salary)
  {
    return new Employee
    {
      Id = Guid.CreateVersion7(),
      UserId = userId,
      Position = position,
      Salary = salary
    };
  }
}

