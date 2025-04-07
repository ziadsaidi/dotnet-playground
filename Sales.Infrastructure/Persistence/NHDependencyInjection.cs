using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sales.Application.Interfaces;
using NHibernate;
using Microsoft.Extensions.Hosting;
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Application.Products;
using Sales.Persistence.NH.UnitOfWork;
using Sales.Persistence.NH.Repositories;
using Sales.Persistence.NH.Data.Configuration;
using FluentMigrator.Runner;
using Microsoft.Extensions.Logging;
using Sales.Persistence.NH.Migrations;

namespace Sales.Infrastructure.Persistence;

public static class NHDependencyInjection
{
  public static IServiceCollection AddNHibernatePersistence(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    string connectionString = configuration.GetConnectionString("NHibernateConnection")
        ?? throw new InvalidOperationException("NHibernate Database connection string is not configured.");

 // Register the common database connection for Dapper and NHibernate
    _ = services.AddFluentMigratorCore()
        .ConfigureRunner(runner => runner
            .AddPostgres()
            .WithGlobalConnectionString(configuration.GetConnectionString("NHibernateConnection")
                                    ?? throw new InvalidOperationException("NHibernate Database connection string is not configured."))
            .ScanIn(typeof(CustomerMigration).Assembly).For.Migrations())
        .AddLogging(config => config.AddConsole());
    // Configure NHibernate
    services.AddSingleton(provider =>
    {
      ISessionFactory sessionFactory = NHibernateHelper.CreateSessionFactory(connectionString);
      IHostApplicationLifetime lifetime = provider.GetRequiredService<IHostApplicationLifetime>();

      lifetime.ApplicationStopping.Register(() => sessionFactory.Dispose());

      return sessionFactory;
    });

    services.AddScoped(factory => factory.GetRequiredService<ISessionFactory>().OpenSession());

    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    return services;
  }
}

