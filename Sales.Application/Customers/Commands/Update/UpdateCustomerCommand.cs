using Sales.Application.Common;
using Sales.Application.Mediator;

namespace Sales.Application.Customers.Commands.Update;

public record UpdateCustomerCommand(Guid Id, string Name) : IRequest<Unit>;
