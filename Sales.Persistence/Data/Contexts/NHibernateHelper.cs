using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using Microsoft.Extensions.DependencyInjection;
using Sales.Persistence.Configurations.NHibernate;
using FluentMigrator.Runner;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Humanizer;

namespace Sales.Persistence.Data.Contexts
{
  public static class NHibernateHelper
  {
    public static ISessionFactory CreateSessionFactory(string connectionString, IServiceProvider serviceProvider)
    {
      // Configure NHibernate with Fluent NHibernate
      var configuration = Fluently.Configure()
          .Database(PostgreSQLConfiguration.Standard
              .ConnectionString(connectionString)
              .Driver<NpgsqlDriver>()
              .Dialect<PostgreSQLDialect>()

              .ShowSql() // For logging SQL queries
          )
          .Mappings(m =>
          {
            // Register all the mappings explicitly
            _ = m.FluentMappings.AddFromAssemblyOf<CustomerMapping>();
            _ = m.FluentMappings.AddFromAssemblyOf<EmployeeMapping>();
            _ = m.FluentMappings.AddFromAssemblyOf<ProductMapping>();
            _ = m.FluentMappings.AddFromAssemblyOf<SaleMapping>();
            _ = m.FluentMappings.Conventions.Add<SnakeCaseConvention>();
          })
          .ExposeConfiguration(cfg =>
          {
            // Optional: Automatically generate the schema (you can remove this in production)
            // new SchemaExport(cfg).Create(true, true);
          })
          .BuildConfiguration();

      // Build the session factory
      var sessionFactory = configuration.BuildSessionFactory();

      // Apply FluentMigrator migrations when the application starts
      ApplyMigrations(serviceProvider);

      return sessionFactory;
    }

    private static void ApplyMigrations(IServiceProvider serviceProvider)
    {
      // Apply migrations using FluentMigrator
      using var scope = serviceProvider.CreateScope();
      var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
      runner.MigrateUp();
    }
    public class SnakeCaseConvention : IPropertyConvention, IClassConvention
    {
      public void Apply(IPropertyInstance instance)
      {
        // Convert property names to snake_case using Humanizer
        instance.Column(instance.Property.Name.Underscore());
      }

      public void Apply(IClassInstance instance)
      {
        // Convert table names to snake_case using Humanizer
        instance.Table(instance.EntityType.Name.Underscore());
      }
    }
  }
}

