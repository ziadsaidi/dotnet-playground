using ErrorOr;

namespace Sales.Domain.Errors;

public static partial class DomainErrors
{
  public static class UserErrors
  {
    public static Error DuplicateName => Error.Conflict(
        code: "User.DuplicateName",
        description: "A User with the same name already exists");

    public static Error NotFound => Error.NotFound(
        code: "User.NotFound",
        description: "The user with the specified identifier was not found");

  }
}
