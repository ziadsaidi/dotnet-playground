using Sales.Domain.Entities;

namespace Sales.Application.Employees
{
  public interface IEmployeeRepository
  {
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    IAsyncEnumerable<Employee> GetEmployees();

    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(Employee employee, CancellationToken cancellationToken);
  }
}