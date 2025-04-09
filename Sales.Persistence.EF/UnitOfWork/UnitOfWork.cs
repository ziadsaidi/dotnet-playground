
using Sales.Application.Interfaces;
using Sales.Persistence.EF.Data.Configuration;
using Sales.Persistence.EF.Repositories;

namespace Sales.Persistence.EF.UnitOfWork
{
  public class UnitOfWork(AppDbContext context) : IUnitOfWork
  {
    private readonly AppDbContext _context = context;
    private ICustomerRepository? _customerRepository;
    private IEmployeeRepository? _employeeRepository;

    private IProductRepository? _productRepository;

    private IUserRepository? _userRepository;

    public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);

    public IEmployeeRepository Employees => _employeeRepository ??= new EmployeeRepository(_context);

    public IProductRepository Products => _productRepository ??= new ProductRepository(_context);

    public IUserRepository Users => _userRepository ??= new UserRepository(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      return _context.SaveChangesAsync(cancellationToken);
    }
  }
}
