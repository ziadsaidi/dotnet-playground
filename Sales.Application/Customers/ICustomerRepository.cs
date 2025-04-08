using Sales.Domain.Entities;

namespace Sales.Application.Customers
{
  public interface ICustomerRepository
  {
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    IAsyncEnumerable<Customer> GetCustomers();

    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(Customer customer, CancellationToken cancellationToken);

    void Update(Customer customer);

    Task DeleteAsync(Customer customer, CancellationToken cancellationToken);
  }
}
