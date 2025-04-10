using FluentValidation;

namespace Sales.Application.Customers.Commands.Update;

public sealed class UpdateCustomerModelValidator : AbstractValidator<UpdateCustomerCommand>
{
  public UpdateCustomerModelValidator()
  {
    _ = RuleFor(x => x.Address)
        .NotEmpty().WithMessage("Address is required")
        .MaximumLength(100).WithMessage("Address cannot exceed 100 characters")
        .MinimumLength(2).WithMessage("Address must be at least 2 characters");
  }
}
