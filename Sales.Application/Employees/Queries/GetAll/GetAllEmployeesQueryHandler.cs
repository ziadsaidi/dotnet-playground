using ErrorOr;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
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
      return Errors.EmployeeErrors.NotFound;

    return employees.ConvertAll(customer => new EmployeeResponse(
        customer.Id,
        customer.Name));
  }
}

