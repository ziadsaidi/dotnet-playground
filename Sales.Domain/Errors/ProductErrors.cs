using ErrorOr;

namespace Sales.Domain.Errors;

public static partial class DomainErrors
{
  public static class ProductErrors
  {
    public static Error DuplicateName => Error.Conflict(
        code: "Product.DuplicateName",
        description: "A Product with the same name already exists");

    public static Error NotFound => Error.NotFound(
        code: "Product.NotFound",
        description: "The Product with the specified identifier was not found");
  }
}
