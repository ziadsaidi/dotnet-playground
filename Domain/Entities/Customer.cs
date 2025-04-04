using Domain.Common;

namespace Domain.Entities;
public sealed class Customer : IEntity
{
  public Guid Id { get; } = Guid.CreateVersion7();

  public required string Name { get; init; }

  public ICollection<Sale> Sales { get; init; } = [];
}
