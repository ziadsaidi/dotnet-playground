using Sales.Application.Abstractions.Mediator;
using Sales.Application.Customers.Common.Responses;

namespace Sales.Application.Customers.Queries.GetById;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerResponse?>;
