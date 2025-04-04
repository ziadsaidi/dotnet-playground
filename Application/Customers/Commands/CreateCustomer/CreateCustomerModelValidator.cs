using FluentValidation;

namespace Application.Customers.Commands.CreateCustomer;

public class CreateCustomerModelValidator : AbstractValidator<CreateCustomerModel>
{
  public CreateCustomerModelValidator()
  {
    RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required")
        .MaximumLength(100).WithMessage("Name cannot exceed 100 characters")
        .MinimumLength(2).WithMessage("Name must be at least 2 characters");
  }
}