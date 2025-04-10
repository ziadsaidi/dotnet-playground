using ErrorOr;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Errors;

namespace Sales.Application.Employees.Queries.GetAll;

public sealed class GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllEmployeesQuery, List<EmployeeResponse>>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<ErrorOr<List<EmployeeResponse>>> HandleAsync(GetAllEmployeesQuery request, CancellationToken cancellationToken)
  {
    List<Employee> employees = await _unitOfWork.Employees.GetEmployees().ToListAsync(cancellationToken);

    if (employees.Count == 0)
    {
      return DomainErrors.EmployeeErrors.NotFound;
    }

    return employees.ConvertAll(static customer => new EmployeeResponse(
        customer.Id,
        customer.User.FullName));
  }
}
