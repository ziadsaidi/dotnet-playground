using System.Data;
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Application.Interfaces;
using Sales.Application.Products;
using Sales.Persistence.Dapper.Repositories;

namespace Sales.Persistence.Dapper.UnitOfWork;

public class UnitOfWork(IDbConnection dbConnection) : IUnitOfWork
{
  private readonly IDbConnection _dbConnection = dbConnection;
  private IDbTransaction? _transaction;

  public ICustomerRepository Customers { get; } = new CustomerRepository(dbConnection);

  public IEmployeeRepository Employees { get; } = new EmployeeRepository(dbConnection);

  public IProductRepository Products { get; } = new ProductRepository(dbConnection);

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
