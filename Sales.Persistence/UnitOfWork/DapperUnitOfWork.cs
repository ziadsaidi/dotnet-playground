using System.Data;
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Application.Interfaces;
using Sales.Persistence.Repositories.Dapper;
namespace Sales.Persistence.UnitOfWork;

public class DapperUnitOfWork(IDbConnection dbConnection) : IUnitOfWork
{
  private readonly IDbConnection _dbConnection = dbConnection;
  private IDbTransaction? _transaction;

  public ICustomerRepository Customers { get; } = new DapperCustomerRepository(dbConnection);

  public IEmployeeRepository Employees { get; } = new DapperEmployeeRepository(dbConnection);

  public void BeginTransaction()
  {
    _transaction = _dbConnection.BeginTransaction();
  }

  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    _transaction?.Commit();
    _transaction = null;
    return await Task.FromResult(1); // Dapper ne gère pas `SaveChanges`, on retourne 1 par convention
  }

  public void Dispose()
  {
    _transaction?.Dispose();
    _dbConnection?.Dispose();
  }
}
