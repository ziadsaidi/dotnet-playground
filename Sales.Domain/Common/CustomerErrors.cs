using ErrorOr;

namespace Sales.Domain.Common;

public static partial class Errors
{
  public static class CustomerErrors
  {
    public static Error DuplicateName => Error.Conflict(
        code: ErrorCodes.Customer.DuplicateName,
        description: "A customer with the same name already exists");

    public static Error NotFound => Error.NotFound(
        code: ErrorCodes.Customer.NotFound,
        description: "The customer with the specified identifier was not found");
  }
}
