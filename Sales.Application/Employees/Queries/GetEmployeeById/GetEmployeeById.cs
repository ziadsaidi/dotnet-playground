using ErrorOr;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Common;
using Sales.Domain.Entities;

namespace Sales.Application.Employees.Queries.GetEmployeeById
{
  public class GetEmployeeById(IUnitOfWork unitOfWork) : IGetEmployeeById
  {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<EmployeeResponse?>> ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
      Employee? customer = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);

      return customer is null
          ? Errors.EmployeeErrors.NotFound
          : new EmployeeResponse(customer.Id, customer.Name);
    }
  }
}
