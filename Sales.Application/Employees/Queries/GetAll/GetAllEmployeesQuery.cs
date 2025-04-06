using Sales.Application.Employees.Common.Responses;
using Sales.Application.Mediator;

namespace Sales.Application.Employees.Queries.GetAll;

public record GetAllEmployeesQuery : IRequest<List<EmployeeResponse>>;
