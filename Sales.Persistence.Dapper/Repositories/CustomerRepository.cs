using System.Data;
using Dapper;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Persistence.Dapper.Repositories;

public class CustomerRepository(IDbConnection dbConnection) : ICustomerRepository
{
  private readonly IDbConnection _dbConnection = dbConnection;

  public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    const string sql = "SELECT * FROM customers WHERE id = @Id";
    return _dbConnection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id });
  }

  public async IAsyncEnumerable<Customer> GetCustomers()
  {
    const string sql = "SELECT * FROM customers";
    using var reader = await _dbConnection.ExecuteReaderAsync(sql);

    while (reader.Read())
    {
      yield return Customer.Create(
        userId: reader.GetGuid(reader.GetOrdinal("user_id")),
        address: reader.GetString(reader.GetOrdinal("address")));
    }
  }

  public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    const string sql = "SELECT COUNT(1) FROM customers WHERE name = @Name";
    var count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { Name = name });
    return count > 0;
  }

  public Task AddAsync(Customer customer, CancellationToken cancellationToken)
  {
    const string sql = "INSERT INTO customers (id, name) VALUES (@Id, @Name)";
    return _dbConnection.ExecuteAsync(sql, customer);
  }

  public void Update(Customer customer)
  {
    const string sql = "UPDATE customers SET name = @Name WHERE id = @Id";
    _dbConnection.Execute(sql, customer);
  }

  public Task DeleteAsync(Customer customer, CancellationToken cancellationToken)
  {
    const string sql = "DELETE FROM customers WHERE id = @Id";
    return _dbConnection.ExecuteAsync(sql, new { customer.Id });
  }

  public Task<Customer?> GetByIdWithUserAsync(Guid id, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}