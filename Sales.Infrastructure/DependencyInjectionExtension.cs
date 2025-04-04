using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NHibernate;
using Sales.Application.Customers;
using Sales.Application.Employees;
using Sales.Application.Interfaces;
using Sales.Persistence.Data.Contexts;
using Sales.Persistence.Repositories.EntityFramework;
using Sales.Persistence.Repositories.NHibernate;
using Sales.Persistence.UnitOfWork;

namespace Sales.Infrastructure;

public enum DatabaseProvider
{
  EntityFramework,
  NHibernate
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
    string connectionString = configuration.GetConnectionString("NHibernateConnection")
        ?? throw new InvalidOperationException("NHibernate Database connection string is not configured.");

    // Configure FluentMigrator for running migrations
    _ = services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Persistence.NHibernateMigrations.InitialNHibernateMigration).Assembly).For.Migrations())
            .AddLogging(config => config.AddConsole());

    // Configure NHibernate
    _ = services.AddSingleton(provider =>
    {
      ISessionFactory sessionFactory = NHibernateHelper.CreateSessionFactory(connectionString);
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
}