using ErrorOr;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Interfaces;
using Sales.Application.Products.Common.Responses;
using Sales.Domain.Errors;

namespace Sales.Application.Products.Queries.GetAll;

public class GetAllProductsQueryhandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<ErrorOr<List<ProductResponse>>> HandleAsync(GetAllProductsQuery request, CancellationToken cancellationToken)
  {
    var products = await _unitOfWork.Products.GetProducts().ToListAsync(cancellationToken);

    if (products.Count == 0)
    {
      return DomainErrors.ProductErrors.NotFound;
    }

    return products.ConvertAll(static p => new ProductResponse(p.Id, p.Name, p.Price.GetValueOrDefault()));
  }
}
