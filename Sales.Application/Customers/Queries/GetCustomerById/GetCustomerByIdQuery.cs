using ErrorOr;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Common;

namespace Sales.Application.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQuery(IUnitOfWork unitOfWork) : IGetCustomerByIdQuery
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<ErrorOr<CustomerResponse?>> ExecuteAsync(Guid id, CancellationToken cancellationToken)
  {
    var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken);

    return customer is null
        ? Errors.CustomerErrors.NotFound
        : new CustomerResponse(customer.Id, customer.Name);
  }
}
