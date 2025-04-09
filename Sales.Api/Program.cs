using System.Diagnostics;
using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NHibernate.Dialect.Function;
using Sales.API.Authorization;
using Sales.Application.Common;
using Sales.Application.Customers.Commands.Create;
using Sales.Application.Customers.Commands.Delete;
using Sales.Application.Customers.Commands.Update;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Customers.Queries.GetAll;
using Sales.Application.Customers.Queries.GetById;
using Sales.Application.Employees.Commnads.Create;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Employees.Queries.GetAll;
using Sales.Application.Employees.Queries.GetById;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
using Sales.Application.Products.Commands.Create;
using Sales.Application.Products.Common.Responses;
using Sales.Application.Products.Queries.GetAll;
using Sales.Application.Products.Queries.GetById;
using Sales.Application.Users.Commands.Login;
using Sales.Application.Users.Commands.Register;
using Sales.Infrastructure;
using Sales.Persistence.Dapper.Data.Configuration;
using Sales.Persistence.EF.Data.Configuration;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Get database provider from configuration
string providerName = builder.Configuration["DatabaseProvider"] ?? "EntityFramework";
DatabaseProvider databaseProvider = Enum.TryParse<DatabaseProvider>(providerName, true, out DatabaseProvider provider)
    ? provider
    : DatabaseProvider.EntityFramework;

_ = builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Exception handling
_ = builder.Services.AddProblemDetails(options =>
{
  IWebHostEnvironment environment = builder.Environment;
  options.CustomizeProblemDetails = context =>
          {
            context.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            if (!environment.IsDevelopment())
            {
              context.ProblemDetails.Detail = "An error occurred. Contact support if the problem persists.";
            }
          };
});
// Register Infrastructure with the selected provider
builder.Services.AddPersistence(builder.Configuration, databaseProvider);

// Load secret from configuration
string jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? "DefaultSuperSecretKey";

// Register Infrastructure Services
builder.Services.AddInfrastructure(jwtSecret);

// CORS configuration
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(builder.Configuration["AllowedOrigins"]?.Split(',') ?? [])));

// Register the Mediator
builder.Services.AddScoped<IAppMediator, AppMediator>();

// Register all handlers (queries and commands)
//customers
builder.Services.AddScoped<IRequestHandler<CreateCustomerCommand, CustomerResponse?>, CreateCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCustomerCommand, Unit>, UpdateCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteCustomerCommand, Unit>, DeleteCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllCustomersQuery, List<CustomerResponse>>, GetAllCustomersQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetCustomerByIdQuery, CustomerResponse?>, GetCustomerByIdQueryHandler>();
//employees
builder.Services.AddScoped<IRequestHandler<CreateEmployeeCommand, EmployeeResponse?>, CreateEmployeeCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllEmployeesQuery, List<EmployeeResponse>>, GetAllEmployeesQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetEmployeeBydQuery, EmployeeResponse?>, GetEmployeeByIdQueryHandler>();
//products
builder.Services.AddScoped<IRequestHandler<CreateProductCommand, ProductResponse?>, CreateProductCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllProductsQuery, List<ProductResponse>>, GetAllProductsQueryhandler>();
builder.Services.AddScoped<IRequestHandler<GetProductByIdQuery, ProductResponse?>, GetProductByIdQueryHandler>();
//users
builder.Services.AddScoped<IRequestHandler<RegisterUserCommand, string>, RegisterUserCommandHandler>();
builder.Services.AddScoped<IRequestHandler<LoginUserCommand, string>, LoginUserCammandHandler>();

builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerModelValidator).Assembly);

// Add Controllers
builder.Services.AddControllers();

// Security headers
// _ = builder.Services
//     .AddSecurityHeaderPolicies()
//     .SetPolicySelector(_ => builder.Environment.IsDevelopment()
//         ? new HeaderPolicyCollection()
//             .AddFrameOptionsDeny()
//             .AddContentTypeOptionsNoSniff()
//             .AddStrictTransportSecurityMaxAge()
//             .RemoveServerHeader()
//             .AddReferrerPolicyNoReferrer()
//             .AddPermissionsPolicyWithDefaultSecureDirectives()
//         : new HeaderPolicyCollection().AddDefaultApiSecurityHeaders());

WebApplication app = builder.Build();

// Exception handling
_ = app.UseStatusCodePages();
_ = app.Environment.IsDevelopment()
    ? app.UseDeveloperExceptionPage()
    : app.UseExceptionHandler();

// Middleware
app.UseRouting()
   .UseCors()
   .UseAuthentication()
   .UseAuthorization()
   .UseEndpoints(endpoints => endpoints.MapControllers());

app.MapOpenApi();
app.MapScalarApiReference(options =>
options
      .WithTitle("API")
);

// Migrate Database based on selected provider
await using AsyncServiceScope scope = app.Services.CreateAsyncScope();

if (databaseProvider is DatabaseProvider.EntityFramework)
{
  AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  await context.Database.MigrateAsync();
}
else if (databaseProvider is DatabaseProvider.NHibernate)
{
  IMigrationRunner migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
  migrationRunner.MigrateUp();
}
else if (databaseProvider is DatabaseProvider.Dapper)
{
  var connectionstring = builder.Configuration.GetConnectionString("DapperConnection");
  DatabaseInitializer.Initialize(connectionstring!);
}
// Run application
await app.RunAsync();