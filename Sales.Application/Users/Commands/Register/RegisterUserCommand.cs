
using Sales.Application.Mediator;

namespace Sales.Application.Users.Commands.Register;

public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<string>;
