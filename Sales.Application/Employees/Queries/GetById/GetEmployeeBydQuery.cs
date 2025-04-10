using Sales.Application.Abstractions.Mediator;
using Sales.Application.Employees.Common.Responses;

namespace Sales.Application.Employees.Queries.GetById;

public record GetEmployeeBydQuery(Guid Id) : IRequest<EmployeeResponse?>;
