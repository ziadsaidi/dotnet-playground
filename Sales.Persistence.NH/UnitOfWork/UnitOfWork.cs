using NHibernate;
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Application.Interfaces;
using Sales.Application.Products;
using Sales.Persistence.NH.Repositories;

namespace Sales.Persistence.NH.UnitOfWork
{
  public class UnitOfWork : IUnitOfWork, IDisposable
  { private readonly ISession _session;
    private ITransaction? _transaction;
    private ICustomerRepository? _customerRepository;
    private IEmployeeRepository? _employeeRepository;
    private IProductRepository? _productRepository;
    private bool _disposed;

    public UnitOfWork(ISession session)
    {
      _session = session;
      // Begin transaction when UoW is created
      _transaction = _session.BeginTransaction();
    }

    public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_session);
    public IEmployeeRepository Employees => _employeeRepository ??= new EmployeeRepository(_session);

    public IProductRepository Products => _productRepository ??= new ProductRepository(_session);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      try
      {
        // Flush changes to the database
        await _session.FlushAsync(cancellationToken);

        // Commit the transaction
        if (_transaction?.IsActive == true)
        {
          await _transaction.CommitAsync(cancellationToken);
          _transaction = _session.BeginTransaction();
        }

        return 1;
      }
      catch
      {
        // Rollback on error
        if (_transaction?.IsActive == true)
        {
          await _transaction.RollbackAsync(cancellationToken);
          _transaction = _session.BeginTransaction();
        }
        throw;
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
        {
          // Dispose transaction if active
          if (_transaction?.IsActive == true)
          {
            _transaction.Rollback();
            _transaction.Dispose();
          }

          // Don't dispose the session here as it's managed by DI
        }

        _disposed = true;
      }
    }

    ~UnitOfWork()
    {
      Dispose(false);
    }
  }
}