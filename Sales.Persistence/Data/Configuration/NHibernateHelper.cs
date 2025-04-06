using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using Sales.Persistence.Configurations.NHibernate;

namespace Sales.Persistence.Data.Configuration
{
  public static class NHibernateHelper
  {
    public static ISessionFactory CreateSessionFactory(string connectionString)
    {
      NHibernate.Cfg.Configuration configuration = Fluently.Configure()
          .Database(PostgreSQLConfiguration.Standard
              .ConnectionString(connectionString)
              .Driver<NpgsqlDriver>()
              .Dialect<PostgreSQLDialect>()
              .ShowSql()
          )
          .Mappings(m =>
          {
            _ = m.FluentMappings.AddFromAssemblyOf<CustomerMapping>();
            _ = m.FluentMappings.AddFromAssemblyOf<EmployeeMapping>();
            _ = m.FluentMappings.AddFromAssemblyOf<ProductMapping>();
            _ = m.FluentMappings.AddFromAssemblyOf<SaleMapping>();
          })
          .BuildConfiguration();

      // Build the session factory
      return configuration.BuildSessionFactory();
    }
  }
}
