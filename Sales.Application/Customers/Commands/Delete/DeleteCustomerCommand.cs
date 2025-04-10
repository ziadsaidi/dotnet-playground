
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Common;

namespace Sales.Application.Customers.Commands.Delete;

public record DeleteCustomerCommand(Guid Id) : IRequest<Unit>;
