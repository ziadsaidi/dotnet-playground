using Sales.Application.Abstractions.Mediator;
using Sales.Application.Products.Common.Responses;

namespace Sales.Application.Products.Queries.GetById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;
