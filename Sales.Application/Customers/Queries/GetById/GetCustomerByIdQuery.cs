using Sales.Application.Customers.Common.Responses;
using Sales.Application.Mediator;

namespace Sales.Application.Customers.Queries.GetById;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerResponse?>;
