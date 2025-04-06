using ErrorOr;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
using Sales.Domain.Entities;
using Sales.Domain.Errors;

namespace Sales.Application.Employees.Queries.GetById;

public sealed class GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetEmployeeBydQuery, EmployeeResponse?>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<ErrorOr<EmployeeResponse?>> HandleAsync(GetEmployeeBydQuery request, CancellationToken cancellationToken)
  {
    Employee? customer = await _unitOfWork.Employees.GetByIdAsync(request.Id, cancellationToken);

    return customer is null
        ? Errors.EmployeeErrors.NotFound
        : new EmployeeResponse(customer.Id, customer.Name);
  }
}
