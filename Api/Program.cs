using Application.Customers.Commands;
using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Queries.GetAllCustomers;
using Application.Customers.Queries.GetCustomerByIdQuery;
using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Contexts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();  // Active OpenAPI avec Scalar
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"))
           .UseSnakeCaseNamingConvention()
);


_ = builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(builder.Configuration["AllowedOrigins"]?.Split(',') ?? [])));

builder.Services.AddScoped<ICreateCustomerCommand, CreateCustomerCommand>();
builder.Services.AddScoped<IGetAllCustomerQuery, GetAllCustomersQuery>();
builder.Services.AddScoped<IGetCustomerByIdQuery, GetCustomerByIdQuery>();
builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerModelValidator).Assembly);
builder.Services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());

builder.Services.AddControllers();

var app = builder.Build();
app.UseRouting()
            .UseCors()
            .UseEndpoints(endpoints => endpoints.MapControllers());

app.MapOpenApi(); // Active OpenAPI
app.MapScalarApiReference(options => options.WithTitle("API"));

await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await context.Database.MigrateAsync();

await app.RunAsync();
