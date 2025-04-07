
using ErrorOr;
using FluentValidation;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
using Sales.Domain.Entities;
using Sales.Domain.Errors;

namespace Sales.Application.Employees.Commnads.Create;

public sealed class CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateEmployeeCommand> validator) : IRequestHandler<CreateEmployeeCommand, EmployeeResponse?>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IValidator<CreateEmployeeCommand> _validator = validator;
  public async Task<ErrorOr<EmployeeResponse?>> HandleAsync(CreateEmployeeCommand model, CancellationToken cancellationToken)
  {
    // Validate using FluentValidation
    FluentValidation.Results.ValidationResult validationResult = await _validator.ValidateAsync(model, cancellationToken);

    if (!validationResult.IsValid)
    {
      return validationResult.Errors
          .ConvertAll(error => Error.Validation(error.PropertyName, error.ErrorMessage));
    }

    // Check for duplicates
    if (await _unitOfWork.Employees.ExistsAsync(model.Name, cancellationToken))
    {
      return Errors.EmployeeErrors.DuplicateName;
    }

    // Create and persist the customer
    Employee employee = new() { Name = model.Name };

    await _unitOfWork.Employees.AddAsync(employee, cancellationToken);
    _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Return result
    return new EmployeeResponse(employee.Id, employee.Name);
  }
}
