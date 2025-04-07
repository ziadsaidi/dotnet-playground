using ErrorOr;

namespace Sales.Domain.Errors;

public static partial class Errors
{
  public static class CustomerErrors
  {
    public static Error DuplicateName => Error.Conflict(
        code: ErrorCodes.CustomerCodes.DuplicateName,
        description: "A customer with the same name already exists");

    public static Error NotFound => Error.NotFound(
        code: ErrorCodes.CustomerCodes.NotFound,
        description: "The customer with the specified identifier was not found");
  }
}
