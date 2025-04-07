
using Sales.Application.Mediator;
using Sales.Application.Products.Common.Responses;

namespace Sales.Application.Products.Commands.Create;

public sealed record CreateProductCommand(string Name, double Price) : IRequest<ProductResponse?>;