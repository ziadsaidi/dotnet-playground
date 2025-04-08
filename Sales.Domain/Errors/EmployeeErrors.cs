using ErrorOr;

namespace Sales.Domain.Errors;

public static partial class DomainErrors
{
  public static class EmployeeErrors
  {
    public static Error DuplicateName => Error.Conflict(
        code: "Employee.DuplicateName",
        description: "An Employee with the same name already exists");

    public static Error NotFound => Error.NotFound(
        code: "Employee.NotFound",
        description: "The Employee with the specified identifier was not found");
  }
}
