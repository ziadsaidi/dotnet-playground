
using Sales.Application.Customers;

namespace Sales.Application.Interfaces
{
  public interface IUnitOfWork
  {
    ICustomerRepository Customers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
