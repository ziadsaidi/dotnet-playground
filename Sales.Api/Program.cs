using System.Diagnostics;
using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Customers.Commands.CreateCustomer;
using Sales.Application.Customers.Queries.GetAllCustomers;
using Sales.Application.Customers.Queries.GetCustomerById;
using Sales.Application.Employees.Commnads;
using Sales.Application.Employees.Commnads.CreateEmployee;
using Sales.Application.Employees.Queries.GetAllEmpoyees;
using Sales.Application.Employees.Queries.GetEmployeeById;
using Sales.Infrastructure;
using Sales.Persistence.Data.Contexts;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Get database provider from configuration
string providerName = builder.Configuration["DatabaseProvider"] ?? "EntityFramework";
DatabaseProvider databaseProvider = Enum.TryParse<DatabaseProvider>(providerName, true, out DatabaseProvider provider)
    ? provider
    : DatabaseProvider.EntityFramework;

builder.Services.AddOpenApi();  // Enable OpenAPI with Scalar
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Exception handling
_ = builder.Services.AddProblemDetails(options =>
{
  IWebHostEnvironment environment = builder.Environment;
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
builder.Services.AddScoped<ICreateEmployeeCommand, CreateEmployeCommand>();
builder.Services.AddScoped<IGetAllEmployeesQuery, GetAllEmployeesQuery>();
builder.Services.AddScoped<IGetEmployeeById, GetEmployeeById>();


builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerModelValidator).Assembly);

// Add Controllers
builder.Services.AddControllers();

WebApplication app = builder.Build();

// Middleware
app.UseRouting()
   .UseCors()
   .UseEndpoints(endpoints => endpoints.MapControllers());

// OpenAPI
app.MapOpenApi();
app.MapScalarApiReference(options => options.WithTitle("API"));

// Migrate Database based on selected provider
await using AsyncServiceScope scope = app.Services.CreateAsyncScope();

if (databaseProvider == DatabaseProvider.EntityFramework)
{
  AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  await context.Database.MigrateAsync();
}
else if (databaseProvider == DatabaseProvider.NHibernate)
{
  IMigrationRunner migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
  migrationRunner.MigrateUp();
}

// Run application
await app.RunAsync();