using FluentValidation;
using ErrorOr;
using Sales.Application.Customers.Common.Responses;
using Sales.Domain.Entities;
using Sales.Domain.Common;
using Sales.Application.Interfaces;

namespace Sales.Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommand(
    IUnitOfWork unitOfWork,
    IValidator<CreateCustomerModel> validator) : ICreateCustomerCommand
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IValidator<CreateCustomerModel> _validator = validator;

  public async Task<ErrorOr<CustomerResponse?>> ExecuteAsync(CreateCustomerModel model, CancellationToken cancellationToken = default)
  {
    // Validate using FluentValidation
    FluentValidation.Results.ValidationResult validationResult = await _validator.ValidateAsync(model, cancellationToken);

    if (!validationResult.IsValid)
    {
      return validationResult.Errors
          .ConvertAll(error => Error.Validation(error.PropertyName, error.ErrorMessage));
    }

    // Check for duplicates
    if (await _unitOfWork.Customers.ExistsAsync(model.Name, cancellationToken))
    {
      return Errors.CustomerErrors.DuplicateName;
    }

    // Create and persist the customer
    Customer customer = new() { Name = model.Name };

    await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
    _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Return result
    return new CustomerResponse(customer.Id, customer.Name);
  }
}
