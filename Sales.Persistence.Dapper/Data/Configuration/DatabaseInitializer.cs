using DbUp;

namespace Sales.Persistence.Dapper.Data.Configuration;

public static class DatabaseInitializer
{
  public static void Initialize(string connectionString)
  {
    EnsureDatabase.For.PostgresqlDatabase(connectionString);

    var migrator = DeployChanges
    .To
    .PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(typeof(DatabaseInitializer).Assembly)
    .WithTransaction()
    .LogToConsole()
    .Build();

    if (migrator.IsUpgradeRequired())
    {
      var result = migrator.PerformUpgrade();

      if (!result.Successful)
      {
        Console.WriteLine("Database migration Failed : " + result.Error);
      }
    }
  }
}
