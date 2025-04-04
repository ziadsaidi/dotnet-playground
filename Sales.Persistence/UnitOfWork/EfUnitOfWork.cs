
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Application.Interfaces;
using Sales.Persistence.Data.Contexts;
using Sales.Persistence.Repositories.EntityFramework;

namespace Sales.Persistence.UnitOfWork
{
  public class EfUnitOfWork(AppDbContext context) : IUnitOfWork
  {
    private readonly AppDbContext _context = context;
    private ICustomerRepository? _customerRepository;
    private IEmployeeRepository? _employeeRepository;

    public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);

    public IEmployeeRepository Employees => _employeeRepository ??= new EmployeeRepository(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      return _context.SaveChangesAsync(cancellationToken);
    }
  }
}
