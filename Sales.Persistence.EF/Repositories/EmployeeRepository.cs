using Microsoft.EntityFrameworkCore;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Persistence.EF.Data.Configuration;

namespace Sales.Persistence.EF.Repositories;

using EF = Microsoft.EntityFrameworkCore.EF;
public class EmployeeRepository : IEmployeeRepository
{
  private readonly AppDbContext _context;

  public EmployeeRepository(AppDbContext context)
  {
    _context = context;
  }

  public Task AddAsync(Employee employee, CancellationToken cancellationToken)
  {
    return _context.Employees.AddAsync(employee, cancellationToken).AsTask();
  }

  public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
  {
    return _context.Employees
        .Include(e => e.User)
        .AsNoTracking()
        .AnyAsync(e => EF.Functions.Like(e.User.FullName, name), cancellationToken);
  }

  public Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return _context.Employees
      .Include(e => e.User)
      .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
  }

  public IAsyncEnumerable<Employee> GetEmployees()
  {
    return _context.Employees
      .Include(e => e.User)
      .AsNoTracking()
      .AsAsyncEnumerable();
  }
}
