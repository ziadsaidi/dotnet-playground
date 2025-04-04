
using Sales.Application.Customers;
using Sales.Application.Employees;

namespace Sales.Application.Interfaces
{
  public interface IUnitOfWork
  {
    ICustomerRepository Customers { get; }
    IEmployeeRepository Employees { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
