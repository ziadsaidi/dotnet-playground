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
    var lowerCaseName = name.ToLowerInvariant();
#pragma warning disable CA1862 // Utiliser les surcharges de méthode « StringComparison » pour effectuer des comparaisons de chaînes sans respect de la casse
    return _session.Query<Customer>()
                   .AnyAsync(c => c.Name.ToLowerInvariant() == lowerCaseName, cancellationToken);
#pragma warning restore CA1862 // Utiliser les surcharges de méthode « StringComparison » pour effectuer des comparaisons de chaînes sans respect de la casse
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
