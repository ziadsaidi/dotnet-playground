using ErrorOr;
using FluentValidation;
using Sales.Application.Common;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
using Sales.Domain.Errors;

namespace Sales.Application.Customers.Commands.Update;

public class UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateCustomerCommand> validator) : IRequestHandler<UpdateCustomerCommand, Unit>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IValidator<UpdateCustomerCommand> _validator = validator;
  public async Task<ErrorOr<Unit>> HandleAsync(UpdateCustomerCommand request, CancellationToken cancellationToken)
  {
    var validationResult = await _validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
    {
      return validationResult.Errors
          .ConvertAll(static error => Error.Validation(error.PropertyName, error.ErrorMessage));
    }
    var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id, cancellationToken);
    if (customer is null)
    {
      return DomainErrors.CustomerErrors.NotFound;
    }

    customer.Update(request.Name);

    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Unit.Value;
  }
}
