using NHibernate;
using NHibernate.Linq;
using Sales.Application.Customers;
using Sales.Domain.Entities;

namespace Sales.Persistence.Repositories.NHibernate;

public class NHCustomerRepository(ISession session) : ICustomerRepository
{
  private readonly ISession _session = session;

  public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return _session.GetAsync<Customer>(id, cancellationToken)!;
  }

  public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    string lowerCaseName = name.ToLowerInvariant();
    return _session.Query<Customer>()
                   .AnyAsync(c => c.Name.ToLowerInvariant() == lowerCaseName, cancellationToken);
  }

  public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
  {
    _ = await _session.SaveAsync(customer, cancellationToken);
  }
  public IAsyncEnumerable<Customer> GetCustomers()
  {
    return _session.Query<Customer>().ToAsyncEnumerable();
  }
}
