
using ErrorOr;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;

namespace Sales.Application.Users.Commands.Login;

public class LoginUserCammandHandler(IAuthService authService, IUserRepository userRepository) : IRequestHandler<LoginUserCommand, string>
{
  private readonly IAuthService _authService = authService;
  private readonly IUserRepository _userRepository = userRepository;
  public async Task<ErrorOr<string>> HandleAsync(LoginUserCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

    if (user is null)
    {
      return Error.Unauthorized("User not found");
    }
    var isValid = await _authService.ValidateUserAsync(request.Email, request.Password, cancellationToken);

    if (!isValid)
    {
      return Error.Unauthorized("Authentication failed");
    }

    return _authService.GenerateToken(Guid.NewGuid(), request.Email);
  }
}
