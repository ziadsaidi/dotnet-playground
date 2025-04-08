using FluentValidation;

namespace Sales.Application.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
  public CreateProductCommandValidator()
  {
    RuleFor(static x => x.Name)
       .NotEmpty().WithMessage("Name is required")
       .MaximumLength(100).WithMessage("Name cannot exceed 100 characters")
       .MinimumLength(2).WithMessage("Name must be at least 2 characters");

    RuleFor(static x => x.Price)
      .NotEmpty().WithMessage("Price is required")
      .GreaterThan(0).WithMessage("Product price Should be  greater then 0.");
  }
}
