using System.Data;
using Dapper;
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Domain.Entities;
namespace Sales.Persistence.Repositories.Dapper;

public class DapperEmployeeRepository(IDbConnection dbConnection) : IEmployeeRepository
{
  private readonly IDbConnection _dbConnection = dbConnection;

  public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    const string sql = "SELECT * FROM employees WHERE id = @Id";
    return await _dbConnection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
  }

  public async IAsyncEnumerable<Employee> GetEmployees()
  {
    const string sql = "SELECT * FROM employees";
    using var reader = await _dbConnection.ExecuteReaderAsync(sql);

    while (reader.Read())
    {
      yield return new Employee
      {
        Id = reader.GetGuid(reader.GetOrdinal("id")),
        Name = reader.GetString(reader.GetOrdinal("name"))
      };
    }
  }

  public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    const string sql = "SELECT COUNT(1) FROM employees WHERE name = @Name";
    var count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { Name = name });
    return count > 0;
  }

  public async Task AddAsync(Employee employee, CancellationToken cancellationToken)
  {
    const string sql = "INSERT INTO employees (id, name) VALUES (@Id, @Name)";
    await _dbConnection.ExecuteAsync(sql, employee);
  }

}
