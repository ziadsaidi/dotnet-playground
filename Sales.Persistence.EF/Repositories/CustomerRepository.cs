using Microsoft.EntityFrameworkCore;
using Sales.Application.Customers;
using Sales.Domain.Entities;
using Sales.Persistence.EF.Data.Configuration;

namespace Sales.Persistence.EF.Repositories;

public class CustomerRepository : ICustomerRepository
{
  private readonly AppDbContext _context;

  public CustomerRepository(AppDbContext context)
  {
    _context = context;
  }

  public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return _context.Customers.FindAsync([id], cancellationToken).AsTask();
  }

  public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    return _context.Customers
        .AsNoTracking()
        .AnyAsync(c => c.Name == name, cancellationToken);
  }

  public Task AddAsync(Customer customer, CancellationToken cancellationToken)
  {
    return _context.Customers.AddAsync(customer, cancellationToken).AsTask();
  }

  public IAsyncEnumerable<Customer> GetCustomers()
  {
    return _context.Customers.AsNoTracking().AsAsyncEnumerable();
  }

  public void Update(Customer customer)
  {
    _context.Update(customer);
  }

  public Task DeleteAsync(Customer customer, CancellationToken cancellationToken)
  {
    _context.Customers.Remove(customer);
    return Task.CompletedTask;
  }
}