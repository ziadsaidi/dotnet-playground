using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Sales.Api.Exetensions;
using Sales.API.Authorization;
using Sales.Application.Interfaces;
using Sales.Infrastructure;
using Sales.Infrastructure.Identity;
using Sales.Infrastructure.Identity.Options;
using Sales.Persistence.Dapper.Data.Configuration;
using Sales.Persistence.EF.Data.Configuration;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

// --- Configurations ---
var providerName = config["DatabaseProvider"] ?? "EntityFramework";
var databaseProvider = Enum.TryParse<DatabaseProvider>(providerName, true, out var parsedProvider)
    ? parsedProvider
    : DatabaseProvider.EntityFramework;

var allowedOrigins = config["AllowedOrigins"]?.Split(',') ?? [];

// --- Add Core Services ---
services.AddRouting(opt => opt.LowercaseUrls = true);
services.AddHttpContextAccessor();
services.AddControllers();
services.AddAppProblemDetails(env);
services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(allowedOrigins)));

services.AddOpenApi(opt => opt.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

// --- Custom Service Registrations ---
services.AddPersistence(config, databaseProvider);
services.AddInfrastructure(builder.Configuration);
services.AddAppMediators();
services.AddAppValidators();
services.AddAppMappers();
services.AddAppPipelineBehaviors();
services.AddScoped<IUserContext, UserContext>();

var app = builder.Build();

// --- Middleware Pipeline ---
if (env.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.MapOpenApi();
  app.MapScalarApiReference();
}
else
{
  app.UseExceptionHandler();
}

app.UseStatusCodePages();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// --- Database Initialization ---
await using var scope = app.Services.CreateAsyncScope();

switch (databaseProvider)
{
  case DatabaseProvider.EntityFramework:
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    break;
  case DatabaseProvider.NHibernate:
    var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    migrator.MigrateUp();
    break;
  case DatabaseProvider.Dapper:
    var connectionString = config.GetConnectionString("DapperConnection");
    DatabaseInitializer.Initialize(connectionString!);
    break;
}

await app.RunAsync();
