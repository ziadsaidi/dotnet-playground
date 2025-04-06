using Sales.Application.Customers.Common.Responses;
using Sales.Application.Mediator;

namespace Sales.Application.Customers.Queries.GetAll;

public sealed record GetAllCustomersQuery : IRequest<List<CustomerResponse>>;
