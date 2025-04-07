using Microsoft.EntityFrameworkCore;
using Sales.Application.Products;
using Sales.Domain.Entities;
using Sales.Persistence.EF.Data.Configuration;

namespace Sales.Persistence.EF.Repositories;

using EF = Microsoft.EntityFrameworkCore.EF;
public class ProductRepository(AppDbContext context) : IProductRepository
{
  private readonly AppDbContext _context = context;

  public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return _context.Products.FindAsync([id], cancellationToken).AsTask();
  }

  public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    return _context.Products
        .AsNoTracking()
        .AnyAsync(c => EF.Functions.Like(c.Name, name), cancellationToken);
  }

  public Task AddAsync(Product product, CancellationToken cancellationToken)
  {
    return _context.Products.AddAsync(product, cancellationToken).AsTask();
  }

  public IAsyncEnumerable<Product> GetProducts()
  {
    return _context.Products.AsNoTracking().AsAsyncEnumerable();
  }
}
