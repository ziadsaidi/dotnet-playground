using FluentValidation;

namespace Sales.Application.Customers.Commands.Update;

public sealed class UpdateCustomerModelValidator : AbstractValidator<UpdateCustomerCommand>
{
  public UpdateCustomerModelValidator()
  {
    _ = RuleFor(static x => x.Name)
        .NotEmpty().WithMessage("Name is required")
        .MaximumLength(100).WithMessage("Name cannot exceed 100 characters")
        .MinimumLength(2).WithMessage("Name must be at least 2 characters");
  }
}
