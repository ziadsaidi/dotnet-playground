using Sales.Domain.Entities;

namespace Sales.Application.Products;

public interface IProductRepository
{
  Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  IAsyncEnumerable<Product> GetProducts();

  Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);

  Task AddAsync(Product product, CancellationToken cancellationToken);
}
