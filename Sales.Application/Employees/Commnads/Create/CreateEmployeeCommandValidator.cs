using FluentValidation;

namespace Sales.Application.Employees.Commnads.Create;

public sealed class CreateEmployeeModelValidator : AbstractValidator<CreateEmployeeCommand>
{
  public CreateEmployeeModelValidator()
  {
    _ = RuleFor(static x => x.Salary)
        .NotEmpty().WithMessage("Name is required")
        .GreaterThan(0);
  }
}
