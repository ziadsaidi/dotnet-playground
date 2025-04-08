
using Sales.Application.Common;
using Sales.Application.Mediator;

namespace Sales.Application.Customers.Commands.Delete;

public record DeleteCustomerCommand(Guid Id) : IRequest<Unit>;
