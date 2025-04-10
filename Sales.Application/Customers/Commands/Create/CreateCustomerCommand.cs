
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Customers.Common.Responses;

namespace Sales.Application.Customers.Commands.Create;

public record CreateCustomerCommand(string Address) : IRequest<CustomerResponse?>;
