
using Sales.Application.Abstractions.Mediator;
using Sales.Domain.Common;

namespace Sales.Application.Users.Commands.Register;

public record RegisterUserCommand(string Username, string FullName, string Email, string Phone, string Password, Role Role) : IRequest<string>;
