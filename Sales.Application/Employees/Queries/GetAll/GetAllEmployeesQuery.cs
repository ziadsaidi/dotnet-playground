using Sales.Application.Abstractions.Mediator;
using Sales.Application.Employees.Common.Responses;

namespace Sales.Application.Employees.Queries.GetAll;

public record GetAllEmployeesQuery : IRequest<List<EmployeeResponse>>;
