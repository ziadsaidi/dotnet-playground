using Sales.Application.Abstractions.Mediator;
using Sales.Application.Employees.Common.Responses;
using Sales.Domain.Common;

namespace Sales.Application.Employees.Commnads.Create;

public record CreateEmployeeCommand(EmployeePosition Position, double Salary) : IRequest<EmployeeResponse?>;
