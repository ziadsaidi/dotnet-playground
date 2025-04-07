namespace Sales.Domain.Errors;

public static class ErrorCodes
{
  public static class CustomerCodes
  {
    public const string DuplicateName = "Customer.DuplicateName";
    public const string NotFound = "Customer.NotFound";
  }

  public static class EmployeeCodes
  {
    public const string DuplicateName = "Employee.DuplicateName";
    public const string NotFound = "Employee.NotFound";
  }

   public static class ProductCodes
  {
    public const string DuplicateName = "Product.DuplicateName";
    public const string NotFound = "Product.NotFound";
  }
}
