using System.Diagnostics;
using FluentValidation;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Behaviors;
using Sales.Application.Common;
using Sales.Application.Common.Mapping;
using Sales.Application.Customers.Commands.Create;
using Sales.Application.Customers.Commands.Delete;
using Sales.Application.Customers.Commands.Update;
using Sales.Application.Customers.Common.Mapping;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Customers.Queries.GetAll;
using Sales.Application.Customers.Queries.GetById;
using Sales.Application.Employees.Commnads.Create;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Employees.Queries.GetAll;
using Sales.Application.Employees.Queries.GetById;
using Sales.Application.Interfaces;
using Sales.Application.Products.Commands.Create;
using Sales.Application.Products.Common.Responses;
using Sales.Application.Products.Queries.GetAll;
using Sales.Application.Products.Queries.GetById;
using Sales.Application.Services;
using Sales.Application.Users.Commands.Login;
using Sales.Application.Users.Commands.Register;
using Sales.Domain.Entities;

namespace Sales.Api.Exetensions;
public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddAppMediators(this IServiceCollection services)
  {
    // Register Mediator and Handlers
    services.AddScoped<IAppMediator, AppMediator>();

    // Customers
    services.AddScoped<IRequestHandler<CreateCustomerCommand, CustomerResponse?>, CreateCustomerCommandHandler>();
    services.AddScoped<IRequestHandler<UpdateCustomerCommand, Unit>, UpdateCustomerCommandHandler>();
    services.AddScoped<IRequestHandler<DeleteCustomerCommand, Unit>, DeleteCustomerCommandHandler>();
    services.AddScoped<IRequestHandler<GetAllCustomersQuery, List<CustomerResponse>>, GetAllCustomersQueryHandler>();
    services.AddScoped<IRequestHandler<GetCustomerByIdQuery, CustomerResponse?>, GetCustomerByIdQueryHandler>();

    // Employees
    services.AddScoped<IRequestHandler<CreateEmployeeCommand, EmployeeResponse?>, CreateEmployeeCommandHandler>();
    services.AddScoped<IRequestHandler<GetAllEmployeesQuery, List<EmployeeResponse>>, GetAllEmployeesQueryHandler>();
    services.AddScoped<IRequestHandler<GetEmployeeBydQuery, EmployeeResponse?>, GetEmployeeByIdQueryHandler>();

    // Products
    services.AddScoped<IRequestHandler<CreateProductCommand, ProductResponse?>, CreateProductCommandHandler>();
    services.AddScoped<IRequestHandler<GetAllProductsQuery, List<ProductResponse>>, GetAllProductsQueryhandler>();
    services.AddScoped<IRequestHandler<GetProductByIdQuery, ProductResponse?>, GetProductByIdQueryHandler>();

    // Users
    services.AddScoped<IRequestHandler<RegisterUserCommand, string>, RegisterUserCommandHandler>();
    services.AddScoped<IRequestHandler<LoginUserCommand, string>, LoginUserCommandHandler>();

    return services;
  }

  public static IServiceCollection AddAppValidators(this IServiceCollection services)
  {
    services.AddValidatorsFromAssembly(typeof(CreateCustomerModelValidator).Assembly);
    return services;
  }

  public static IServiceCollection AddAppMappers(this IServiceCollection services)
  {
    services.AddScoped<IMapper<Customer, CustomerResponse>, CustomerMapper>();
    services.AddScoped<ITokenExtractorService, TokenExtractorService>();
    return services;
  }

  public static IServiceCollection AddAppPipelineBehaviors(this IServiceCollection services)
  {
    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    return services;
  }

  public static IServiceCollection AddAppProblemDetails(this IServiceCollection services, IWebHostEnvironment env)
  {
    services.AddProblemDetails(options =>
     options.CustomizeProblemDetails = context =>
    {
      context.ProblemDetails.Extensions["traceId"] =
            Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

      if (!env.IsDevelopment())
      {
        context.ProblemDetails.Detail = "An error occurred. Contact support if the problem persists.";
      }
    });
    return services;
  }
}
