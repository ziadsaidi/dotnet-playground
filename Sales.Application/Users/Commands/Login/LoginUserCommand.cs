

using Sales.Application.Abstractions.Mediator;

namespace Sales.Application.Users.Commands.Login;

public record LoginUserCommand(string Email, string Password) : IRequest<string>;
