using FluentValidation;

namespace Sales.Application.Employees.Commnads.Create;

public sealed class CreateEmployeeModelValidator : AbstractValidator<CreateEmployeeCommand>
{
  public CreateEmployeeModelValidator()
  {
    _ = RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required")
        .MaximumLength(100).WithMessage("Name cannot exceed 100 characters")
        .MinimumLength(2).WithMessage("Name must be at least 2 characters");
  }
}
