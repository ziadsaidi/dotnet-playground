
using ErrorOr;
using Sales.Application.Employees.Common.Responses;

namespace Sales.Application.Employees.Commnads.CreateEmployee;

public interface ICreateEmployeeCommand
{
  Task<ErrorOr<EmployeeResponse?>> ExecuteAsync(CreateEmployeeModel model, CancellationToken cancellationToken);
}
