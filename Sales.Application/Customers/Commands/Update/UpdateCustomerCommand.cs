using Sales.Application.Abstractions.Mediator;
using Sales.Application.Common;

namespace Sales.Application.Customers.Commands.Update;

public record UpdateCustomerCommand(Guid Id, string? Address) : IRequest<Unit>;
