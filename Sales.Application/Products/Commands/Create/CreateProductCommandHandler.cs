using ErrorOr;
using FluentValidation;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
using Sales.Application.Products.Common.Responses;
using Sales.Domain.Entities;
using Sales.Domain.Errors;

namespace Sales.Application.Products.Commands.Create;

public sealed class CreateProductCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateProductCommand> validator) : IRequestHandler<CreateProductCommand, ProductResponse?>
{
  private readonly IValidator<CreateProductCommand> _validator = validator;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<ErrorOr<ProductResponse?>> HandleAsync(CreateProductCommand request, CancellationToken cancellationToken)
  {
    // Validate using FluentValidation
    var validationResult = await _validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
    {
      return validationResult.Errors
          .ConvertAll(error => Error.Validation(error.PropertyName, error.ErrorMessage));
    }
    if (await _unitOfWork.Products.ExistsAsync(request.Name, cancellationToken))
    {
      return Errors.ProductErrors.DuplicateName;
    }
    var newProduct = new Product
    {
      Name = request.Name,
      Price = request.Price
    };

    await _unitOfWork.Products.AddAsync(newProduct, cancellationToken);
    _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

    return new ProductResponse(newProduct.Id, newProduct.Name, newProduct.Price.GetValueOrDefault());
  }
}
