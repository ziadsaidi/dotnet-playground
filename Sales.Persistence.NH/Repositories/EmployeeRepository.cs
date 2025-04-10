using NHibernate;
using NHibernate.Linq;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Persistence.NH.Repositories;

public class EmployeeRepository(ISession session) : IEmployeeRepository
{
  private readonly ISession _session = session;
  public Task AddAsync(Employee employee, CancellationToken cancellationToken)
  {
    return _session.SaveAsync(employee, cancellationToken);
  }

  public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    string lowerCaseName = name.ToLowerInvariant();
    return _session.Query<Employee>().AnyAsync(e => e.User.FullName.ToLowerInvariant() == lowerCaseName, cancellationToken);
  }

  public Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return _session.GetAsync<Employee>(id, cancellationToken)!;
  }

  public IAsyncEnumerable<Employee> GetEmployees()
  {
    return _session.Query<Employee>().ToAsyncEnumerable();
  }
}
