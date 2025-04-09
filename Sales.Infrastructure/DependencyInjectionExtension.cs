using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sales.Infrastructure.Persistence;
using Sales.Application.Interfaces;
using Sales.Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

  public static IServiceCollection AddInfrastructure(this IServiceCollection services, string jwtSecret)
  {
    // Register AuthService with proper dependency injection
    services.AddScoped<IAuthService>(provider =>
    {
      var userRepository = provider.GetRequiredService<IUserRepository>();
      return new AuthService(jwtSecret, userRepository);
    });

    // Add authentication services
    _ = services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      var keyBytes = Encoding.UTF8.GetBytes(jwtSecret);
      var signingKey = new SymmetricSecurityKey(keyBytes)
      {
        KeyId = null
      };

      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = "YourIssuer",
        ValidateAudience = true,
        ValidAudience = "YourAudience",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateLifetime = true, 
        ClockSkew = TimeSpan.Zero
      };
      options.Events = new JwtBearerEvents
      {
        OnAuthenticationFailed = context =>
        {
          Console.WriteLine("Authentication failed: " + context.Exception.Message);
          return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
          Console.WriteLine("Token validated successfully.");
          return Task.CompletedTask;
        }
      };
    });
    return services;
  }
}
