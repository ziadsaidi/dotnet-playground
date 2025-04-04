using ErrorOr;

namespace Sales.Domain.Common;

public static partial class Errors
{
  public static class EmployeeErrors
  {
    public static Error DuplicateName => Error.Conflict(
        code: ErrorCodes.EmployeeCodes.DuplicateName,
        description: "An Employee with the same name already exists");

    public static Error NotFound => Error.NotFound(
        code: ErrorCodes.EmployeeCodes.NotFound,
        description: "The Employee with the specified identifier was not found");
  }
}
