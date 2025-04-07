using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Interfaces;
using Sales.Persistence.EF.Data.Configuration;
using Sales.Application.Customers;
using Sales.Persistence.EF.Repositories;
using Sales.Application.Employees;
using Sales.Application.Products;
using Sales.Persistence.EF.UnitOfWork;

namespace Sales.Infrastructure.Persistence;

public static class EFDependencyInjection
{
  public static IServiceCollection AddEntityFrameworkPersistence(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    string connectionString = configuration.GetConnectionString("PostgreSqlConnection")
        ?? throw new InvalidOperationException("EF Database connection string is not configured.");

    services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString)
               .UseSnakeCaseNamingConvention());

    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    return services;
  }
}
