using System.Data;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NHibernate;
using Npgsql;
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Application.Interfaces;
using Sales.Persistence.Data.Configuration;
using Sales.Persistence.Repositories.Dapper;
using Sales.Persistence.Repositories.EntityFramework;
using Sales.Persistence.Repositories.NHibernate;
using Sales.Persistence.UnitOfWork;

namespace Sales.Infrastructure;

public enum DatabaseProvider
{
  EntityFramework,
  NHibernate,
  Dapper
}

public static class DependencyInjectionExtension
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration,
      DatabaseProvider provider = DatabaseProvider.EntityFramework)
  {
    // Register common services

    // Register the database provider based on the selection
    return provider switch
    {
      DatabaseProvider.EntityFramework => services.AddEntityFramework(configuration),
      DatabaseProvider.NHibernate => services.AddNHibernate(configuration),
      DatabaseProvider.Dapper => services.AddDapper(configuration),
      _ => throw new ArgumentOutOfRangeException(nameof(provider), "Unsupported database provider")
    };
  }

  private static IServiceCollection AddEntityFramework(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    string connectionString = configuration.GetConnectionString("PostgreSqlConnection")
        ?? throw new InvalidOperationException("EF Database connection string is not configured.");

    // Configure Entity Framework
    _ = services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString)
               .UseSnakeCaseNamingConvention());

    // Register EF repositories
    _ = services.AddScoped<ICustomerRepository, CustomerRepository>();
    _ = services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    _ = services.AddScoped<IUnitOfWork, EfUnitOfWork>();

    return services;
  }

  private static IServiceCollection AddNHibernate(
    this IServiceCollection services,
     IConfiguration configuration)
  {
    // Register the common database connection for Dapper and NHibernate
    _ = services.AddFluentMigratorCore()
        .ConfigureRunner(runner => runner
            .AddPostgres()
            .WithGlobalConnectionString(configuration.GetConnectionString("NHibernateConnection")
                                    ?? throw new InvalidOperationException("NHibernate Database connection string is not configured."))
            .ScanIn(typeof(Persistence.Migrations.NHibernate.CustomerMigration).Assembly).For.Migrations())
        .AddLogging(config => config.AddConsole());
    // Configure NHibernate
    _ = services.AddSingleton(provider =>
    {
      ISessionFactory sessionFactory = NHibernateHelper.CreateSessionFactory(configuration.GetConnectionString("NHibernateConnection")
                                    ?? throw new InvalidOperationException("NHibernate Database connection string is not configured."));
      IHostApplicationLifetime lifetime = provider.GetRequiredService<IHostApplicationLifetime>();

      // Dispose sessionFactory properly when the application stops
      _ = lifetime.ApplicationStopping.Register(() => sessionFactory.Dispose());

      return sessionFactory;
    });

    _ = services.AddScoped(factory => factory.GetRequiredService<ISessionFactory>().OpenSession());
    _ = services.AddScoped<ICustomerRepository, NHCustomerRepository>();
    _ = services.AddScoped<IEmployeeRepository, NHEmployeeRepository>();
    _ = services.AddScoped<IUnitOfWork, NHUnitOfWork>();

    return services;
  }

  private static IServiceCollection AddDapper(
    this IServiceCollection services,
    IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("DapperConnection")
                                 ?? throw new InvalidOperationException("Dapper Database connection string is not configured.");
    // Register Dapper Database Connection
    _ = services.AddScoped<IDbConnection>(provider => new NpgsqlConnection(connectionString));

    // Register Dapper repositories
    _ = services.AddScoped<ICustomerRepository, DapperCustomerRepository>();
    _ = services.AddScoped<IEmployeeRepository, DapperEmployeeRepository>();
    _ = services.AddScoped<IUnitOfWork, DapperUnitOfWork>();

    return services;
  }
}
