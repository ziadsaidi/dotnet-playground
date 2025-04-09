using FluentValidation;

namespace Sales.Application.Users.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
  public LoginUserCommandValidator()
  {
    _ = RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Invalid email format.");

    _ = RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(6).WithMessage("Password should be at least 6 characters long.")
        .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
        .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
        .Matches("[0-9]").WithMessage("Password must contain at least one numeric value.")
        .Matches(@"[\W_]").WithMessage("Password must contain at least one special character (e.g., !, @, #, $, etc.).");
  }
}
