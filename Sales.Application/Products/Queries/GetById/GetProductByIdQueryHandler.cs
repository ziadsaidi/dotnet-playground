using ErrorOr;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Interfaces;
using Sales.Application.Products.Common.Responses;
using Sales.Domain.Errors;

namespace Sales.Application.Products.Queries.GetById;

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<ErrorOr<ProductResponse?>> HandleAsync(GetProductByIdQuery request, CancellationToken cancellationToken)
  {
    var product = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
    if (product is null)
    {
      return DomainErrors.ProductErrors.NotFound;
    }

    return new ProductResponse(product.Id, product.Name, product.Price.GetValueOrDefault());
  }
}
