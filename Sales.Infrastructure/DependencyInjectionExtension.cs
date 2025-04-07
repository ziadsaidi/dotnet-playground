using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sales.Infrastructure.Persistence;
namespace Sales.Infrastructure;

public enum DatabaseProvider
{
  EntityFramework,
  NHibernate,
  Dapper
}

public static class PersistenceDependencyInjection
{
  public static IServiceCollection AddPersistence(
      this IServiceCollection services,
      IConfiguration configuration,
      DatabaseProvider provider = DatabaseProvider.EntityFramework)
  {
    return provider switch
    {
      DatabaseProvider.EntityFramework => services.AddEntityFrameworkPersistence(configuration),
      DatabaseProvider.NHibernate => services.AddNHibernatePersistence(configuration),
      DatabaseProvider.Dapper => services.AddDapperPersistence(configuration),
      _ => throw new ArgumentOutOfRangeException(nameof(provider), "Unsupported database provider")
    };
  }
}
