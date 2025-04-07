using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sales.Application.Interfaces;
using System.Data;
using Npgsql;
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Persistence.Dapper.UnitOfWork;
using Sales.Persistence.Dapper.Repositories;

namespace Sales.Infrastructure.Persistence;

public static class DapperDependencyInjection
{
  public static IServiceCollection AddDapperPersistence(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    string connectionString = configuration.GetConnectionString("DapperConnection")
        ?? throw new InvalidOperationException("Dapper Database connection string is not configured.");

    services.AddScoped<IDbConnection>(provider => new NpgsqlConnection(connectionString));

    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    services.AddScoped<Application.Products.IProductRepository, ProductRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    return services;
  }
}
