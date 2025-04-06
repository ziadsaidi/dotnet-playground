
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Mediator;

namespace Sales.Application.Customers.Commands.Create;

public record CreateCustomerCommand(string Name) : IRequest<CustomerResponse?>;
