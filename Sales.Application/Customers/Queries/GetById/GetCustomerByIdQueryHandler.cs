using ErrorOr;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
using Sales.Domain.Entities;
using Sales.Domain.Errors;

namespace Sales.Application.Customers.Queries.GetById;

public sealed class GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCustomerByIdQuery, CustomerResponse?>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<ErrorOr<CustomerResponse?>> HandleAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken)
  {
    Customer? customer = await _unitOfWork.Customers.GetByIdAsync(query.Id, cancellationToken);

    return customer is null
        ? Errors.CustomerErrors.NotFound
        : new CustomerResponse(customer.Id, customer.Name);
  }
}
