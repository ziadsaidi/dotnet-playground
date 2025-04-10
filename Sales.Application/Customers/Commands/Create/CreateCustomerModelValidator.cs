using FluentValidation;

namespace Sales.Application.Customers.Commands.Create;

public sealed class CreateCustomerModelValidator : AbstractValidator<CreateCustomerCommand>
{
  public CreateCustomerModelValidator()
  {
    _ = RuleFor(static x => x.Address)
        .NotEmpty().WithMessage("Address is required")
        .MaximumLength(100).WithMessage("Address cannot exceed 100 characters")
        .MinimumLength(2).WithMessage("Address must be at least 2 characters");
  }
}
