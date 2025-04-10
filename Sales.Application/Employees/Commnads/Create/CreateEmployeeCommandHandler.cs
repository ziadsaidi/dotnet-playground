
using ErrorOr;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Employees.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Application.Employees.Commnads.Create;

public sealed class CreateEmployeeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateEmployeeCommand, EmployeeResponse?>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<ErrorOr<EmployeeResponse?>> HandleAsync(CreateEmployeeCommand model, CancellationToken cancellationToken)
  {
    // Create and persist the customer
    Employee employee = new() { Position = model.Position, Salary = model.Salary };

    await _unitOfWork.Employees.AddAsync(employee, cancellationToken);
    _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Return result
    return new EmployeeResponse(employee.Id, employee.User.FullName);
  }
}
