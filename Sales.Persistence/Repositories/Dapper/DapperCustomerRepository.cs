using System.Data;
using Dapper;
using Sales.Application.Customers;
using Sales.Domain.Entities;
namespace Sales.Persistence.Repositories.Dapper;

public class DapperCustomerRepository(IDbConnection dbConnection) : ICustomerRepository
{
  private readonly IDbConnection _dbConnection = dbConnection;

  public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    const string sql = "SELECT * FROM Customers WHERE id = @Id";
    return await _dbConnection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id });
  }

  public async IAsyncEnumerable<Customer> GetCustomers()
  {
    const string sql = "SELECT * FROM customers";
    using var reader = await _dbConnection.ExecuteReaderAsync(sql);

    while (reader.Read())
    {
      yield return new Customer
      {
        Id = reader.GetGuid(reader.GetOrdinal("id")),
        Name = reader.GetString(reader.GetOrdinal("name"))
      };
    }
  }

  public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    const string sql = "SELECT COUNT(1) FROM customers WHERE name = @Name";
    var count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { Name = name });
    return count > 0;
  }

  public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
  {
    const string sql = "INSERT INTO customers (id, name) VALUES (@Id, @Name)";
    await _dbConnection.ExecuteAsync(sql, customer);
  }
}
