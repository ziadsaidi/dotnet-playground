
using ErrorOr;
using FluentValidation;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Errors;

namespace Sales.Application.Users.Commands.Register;

public class RegisterUserCommandHandler(IUnitOfWork unitOfWork, IValidator<RegisterUserCommand> validator, IAuthService authService) : IRequestHandler<RegisterUserCommand, string>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IValidator<RegisterUserCommand> _validator = validator;
  private readonly IAuthService _authService = authService;
  public async Task<ErrorOr<string>> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken)
  {
    var validationResult = await _validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
    {
      return validationResult.Errors.ConvertAll(v => Error.Validation(v.PropertyName, v.ErrorMessage));
    }
    var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);

    if (user is not null)
    {
      return DomainErrors.UserErrors.DuplicateName;
    }
    // Hash password
    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

    var newUser = User.Create(request.Username, request.FullName, request.Email, request.Phone, passwordHash, request.Role);

    await _unitOfWork.Users.AddAsync(newUser, cancellationToken);

    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return _authService.GenerateToken(newUser.Id, newUser.Email);
  }
}
