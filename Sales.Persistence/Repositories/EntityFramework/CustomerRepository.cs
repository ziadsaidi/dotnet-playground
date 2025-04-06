
using Microsoft.EntityFrameworkCore;
using Sales.Application.Customers;
using Sales.Domain.Entities;
using Sales.Persistence.Data.Configuration;


namespace Sales.Persistence.Repositories.EntityFramework
{
  public class CustomerRepository(AppDbContext context) : ICustomerRepository
  {
    private readonly AppDbContext _context = context;

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      return _context.Customers.FindAsync([id], cancellationToken).AsTask();
    }

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
    {
      return _context.Customers
          .AsNoTracking()
          .AnyAsync(c => EF.Functions.Like(c.Name, name), cancellationToken);
    }

    public Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
      return _context.Customers.AddAsync(customer, cancellationToken).AsTask();
    }

    public IAsyncEnumerable<Customer> GetCustomers()
    {
      return _context.Customers.AsNoTracking().AsAsyncEnumerable();
    }
  }
}
