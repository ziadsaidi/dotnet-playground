using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Sales.Persistence.EF.Data.Configuration;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
  public AppDbContext CreateDbContext(string[] args)
  {
    // Build configuration for connection string
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

    optionsBuilder.UseNpgsql(
        configuration.GetConnectionString("PostgreSqlConnection"))
        .UseSnakeCaseNamingConvention();

    return new AppDbContext(optionsBuilder.Options);
  }
}