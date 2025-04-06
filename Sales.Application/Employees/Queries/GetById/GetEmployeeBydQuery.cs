using Sales.Application.Employees.Common.Responses;
using Sales.Application.Mediator;

namespace Sales.Application.Employees.Queries.GetById;

public record GetEmployeeBydQuery(Guid Id) : IRequest<EmployeeResponse?>;
