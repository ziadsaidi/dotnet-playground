using Sales.Application.Abstractions.Mediator;
using Sales.Application.Products.Common.Responses;

namespace Sales.Application.Products.Queries.GetAll;

public record GetAllProductsQuery : IRequest<List<ProductResponse>>;