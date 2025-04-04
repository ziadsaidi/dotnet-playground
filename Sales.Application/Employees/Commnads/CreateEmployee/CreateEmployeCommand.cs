
using ErrorOr;
using FluentValidation;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Common;
using Sales.Domain.Entities;

namespace Sales.Application.Employees.Commnads.CreateEmployee;

public class CreateEmployeCommand(IUnitOfWork unitOfWork, IValidator<CreateEmployeeModel> validator) : ICreateEmployeeCommand
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IValidator<CreateEmployeeModel> _validator = validator;
  public async Task<ErrorOr<EmployeeResponse?>> ExecuteAsync(CreateEmployeeModel model, CancellationToken cancellationToken)
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
