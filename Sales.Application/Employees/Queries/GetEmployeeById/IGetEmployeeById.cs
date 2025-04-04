using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Sales.Application.Employees.Common.Responses;

namespace Sales.Application.Employees.Queries.GetEmployeeById;

public interface IGetEmployeeById
{
  public Task<ErrorOr<EmployeeResponse?>> ExecuteAsync(Guid id, CancellationToken cancellationToken);
}
