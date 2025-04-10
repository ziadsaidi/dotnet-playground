using Sales.Application.Abstractions.Mediator;
using Sales.Application.Customers.Common.Responses;

namespace Sales.Application.Customers.Queries.GetAll;

public sealed record GetAllCustomersQuery : IRequest<List<CustomerResponse>>;
