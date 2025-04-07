using System.Data;
using Dapper;
using Sales.Application.Products;
using Sales.Domain.Entities;
namespace Sales.Persistence.Dapper.Repositories;

public class ProductRepository(IDbConnection dbConnection) : IProductRepository
{
  private readonly IDbConnection _dbConnection = dbConnection;

  public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    const string sql = "SELECT * FROM products WHERE id = @Id";
    return await _dbConnection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
  }

  public async IAsyncEnumerable<Product> GetProducts()
  {
    const string sql = "SELECT * FROM products";
    using var reader = await _dbConnection.ExecuteReaderAsync(sql);

    while (reader.Read())
    {
      yield return new Product
      {
        Id = reader.GetGuid(reader.GetOrdinal("id")),
        Name = reader.GetString(reader.GetOrdinal("name")),
        Price = reader.GetDouble(reader.GetOrdinal("price")),
      };
    }
  }

  public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    const string sql = "SELECT COUNT(1) FROM products WHERE name = @Name";
    var count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { Name = name });
    return count > 0;
  }

  public async Task AddAsync(Product product, CancellationToken cancellationToken)
  {
    const string sql = "INSERT INTO products (id, name) VALUES (@Id, @Name)";
    await _dbConnection.ExecuteAsync(sql, product);
  }
}
