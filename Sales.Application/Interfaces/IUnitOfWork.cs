
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Application.Products;

namespace Sales.Application.Interfaces
{
  public interface IUnitOfWork
  {
    ICustomerRepository Customers { get; }
    IEmployeeRepository Employees { get; }

    IProductRepository Products { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
