using FluentValidation;
using ErrorOr;
using Sales.Application.Customers.Common.Responses;
using Sales.Domain.Entities;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
using Sales.Domain.Errors;

namespace Sales.Application.Customers.Commands.Create;

public sealed class CreateCustomerCommandHandler(
    IUnitOfWork unitOfWork,
    IValidator<CreateCustomerCommand> validator) : IRequestHandler<CreateCustomerCommand, CustomerResponse?>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IValidator<CreateCustomerCommand> _validator = validator;

  public async Task<ErrorOr<CustomerResponse?>> HandleAsync(CreateCustomerCommand model, CancellationToken cancellationToken = default)
  {
    var validationResult = await _validator.ValidateAsync(model, cancellationToken);

    if (!validationResult.IsValid)
    {
      return validationResult.Errors
          .ConvertAll(static error => Error.Validation(error.PropertyName, error.ErrorMessage));
    }

    if (await _unitOfWork.Customers.ExistsAsync(model.Name, cancellationToken))
    {
      return DomainErrors.CustomerErrors.DuplicateName;
    }

    var customer = Customer.Create(model.Name);

    await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
    _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

    return new CustomerResponse(customer.Id, customer.Name);
  }
}
