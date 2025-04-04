using System.Diagnostics;
using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Customers.Commands.CreateCustomer;
using Sales.Application.Customers.Queries.GetAllCustomers;
using Sales.Application.Customers.Queries.GetCustomerById;
using Sales.Infrastructure;
using Sales.Persistence.Data.Contexts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Get database provider from configuration
var providerName = builder.Configuration["DatabaseProvider"] ?? "EntityFramework";
var databaseProvider = Enum.TryParse<DatabaseProvider>(providerName, true, out var provider)
    ? provider
    : DatabaseProvider.EntityFramework;

builder.Services.AddOpenApi();  // Enable OpenAPI with Scalar
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Exception handling
_ = builder.Services.AddProblemDetails(options =>
{
  var environment = builder.Environment;
  options.CustomizeProblemDetails = context =>
          {
            context.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            if (!environment.IsDevelopment())
              context.ProblemDetails.Detail = "An error occurred. Contact support if the problem persists.";
          };
});
// Register Infrastructure with the selected provider
builder.Services.AddInfrastructure(builder.Configuration, databaseProvider);

// CORS configuration
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(builder.Configuration["AllowedOrigins"]?.Split(',') ?? Array.Empty<string>())));

// Register commands and queries
builder.Services.AddScoped<ICreateCustomerCommand, CreateCustomerCommand>();
builder.Services.AddScoped<IGetAllCustomerQuery, GetAllCustomersQuery>();
builder.Services.AddScoped<IGetCustomerByIdQuery, GetCustomerByIdQuery>();
builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerModelValidator).Assembly);

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Middleware
app.UseRouting()
   .UseCors()
   .UseEndpoints(endpoints => endpoints.MapControllers());

// OpenAPI
app.MapOpenApi();
app.MapScalarApiReference(options => options.WithTitle("API"));

// Migrate Database based on selected provider
await using var scope = app.Services.CreateAsyncScope();

if (databaseProvider == DatabaseProvider.EntityFramework)
{
  var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  await context.Database.MigrateAsync();
}
else if (databaseProvider == DatabaseProvider.NHibernate)
{
  var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
  migrationRunner.MigrateUp();
}

// Run application
await app.RunAsync();