using NHibernate;
using NHibernate.Linq;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Persistence.NH.Repositories;

public class ProductRepository(ISession session) : IProductRepository
{
  private readonly ISession _session = session;

  public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return _session.GetAsync<Product>(id, cancellationToken)!;
  }

  public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    string lowerCaseName = name.ToLowerInvariant();
    return _session.Query<Product>()
                   .AnyAsync(c => c.Name.ToLowerInvariant() == lowerCaseName, cancellationToken);
  }

  public async Task AddAsync(Product product, CancellationToken cancellationToken)
  {
    _ = await _session.SaveAsync(product, cancellationToken);
  }
  public IAsyncEnumerable<Product> GetProducts()
  {
    return _session.Query<Product>().ToAsyncEnumerable();
  }
}
