namespace Sales.Application.Interfaces
{
  public interface IUnitOfWork
  {
    ICustomerRepository Customers { get; }
    IEmployeeRepository Employees { get; }

    IProductRepository Products { get; }

    IUserRepository Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
