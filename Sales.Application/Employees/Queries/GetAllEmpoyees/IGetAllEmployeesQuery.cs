
using ErrorOr;
using Sales.Application.Employees.Common.Responses;

namespace Sales.Application.Employees.Queries.GetAllEmpoyees;

public interface IGetAllEmployeesQuery
{
  Task<ErrorOr<List<EmployeeResponse>>> ExecuteAsync(CancellationToken cancellationToken);
}
