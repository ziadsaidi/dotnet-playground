using Sales.Application.Employees.Common.Responses;
using Sales.Application.Mediator;

namespace Sales.Application.Employees.Commnads.Create;

public record CreateEmployeeCommand(string Name) : IRequest<EmployeeResponse?>;
