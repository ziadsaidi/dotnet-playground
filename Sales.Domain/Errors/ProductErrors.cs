using ErrorOr;

namespace Sales.Domain.Errors;

public static partial class Errors
{
  public static class ProductErrors
  {
    public static Error DuplicateName => Error.Conflict(
        code: ErrorCodes.ProductCodes.DuplicateName,
        description: "A Product with the same name already exists");

    public static Error NotFound => Error.NotFound(
        code: ErrorCodes.ProductCodes.NotFound,
        description: "The Product with the specified identifier was not found");
  }
}
