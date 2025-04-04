using Microsoft.EntityFrameworkCore;
using Sales.Application.Employees;
using Sales.Domain.Entities;
using Sales.Persistence.Data.Contexts;

namespace Sales.Persistence.Repositories.EntityFramework
{
  public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
  {
    private readonly AppDbContext _context = context;
    public Task AddAsync(Employee employee, CancellationToken cancellationToken)
    {
      return _context.Employees.AddAsync(employee, cancellationToken).AsTask();
    }

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
    {
      return _context.Employees
          .AsNoTracking()
          .AnyAsync(c => EF.Functions.Like(c.Name, name), cancellationToken);
    }

    public Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      return _context.Employees.FindAsync([id], cancellationToken).AsTask();
    }

    public IAsyncEnumerable<Employee> GetEmployees()
    {
      return _context.Employees.AsNoTracking().AsAsyncEnumerable();
    }
  }
}