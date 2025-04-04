using NHibernate;
using NHibernate.Linq;
using Sales.Application.Employees;
using Sales.Domain.Entities;

namespace Sales.Persistence.Repositories.NHibernate;

public class NHEmployeeRepository(ISession session) : IEmployeeRepository
{
  private readonly ISession _session = session;
  public Task AddAsync(Employee employee, CancellationToken cancellationToken)
  {
    return _session.SaveAsync(employee, cancellationToken);
  }

  public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    string lowerCaseName = name.ToLowerInvariant();
    return _session.Query<Employee>().AnyAsync(e => e.Name.ToLowerInvariant() == lowerCaseName, cancellationToken);
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
