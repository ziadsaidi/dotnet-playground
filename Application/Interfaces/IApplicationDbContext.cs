using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Application.Interfaces;

public interface IApplicationDbContext
{
  DbSet<Sale> Sales { get; }
  DbSet<Customer> Customers { get; }
  DbSet<Employee> Employees { get; }
  DbSet<Product> Products { get; }

  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}