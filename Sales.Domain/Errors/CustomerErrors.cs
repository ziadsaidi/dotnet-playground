using ErrorOr;

namespace Sales.Domain.Errors;

public static partial class DomainErrors
{
  public static class CustomerErrors
  {
    public static Error DuplicateName => Error.Conflict(
        code: "Customer.DuplicateName",
        description: "A customer with the same name already exists");

    public static Error NotFound => Error.NotFound(
        code: "Customer.NotFound",
        description: "The customer with the specified identifier was not found");
  }
}
