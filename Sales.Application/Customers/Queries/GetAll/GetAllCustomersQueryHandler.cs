
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Application.Mediator;
using Sales.Domain.Errors;

namespace Sales.Application.Customers.Queries.GetAll;

public sealed class GetAllCustomersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllCustomersQuery, List<CustomerResponse>>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<ErrorOr<List<CustomerResponse>>> HandleAsync(GetAllCustomersQuery request, CancellationToken cancellationToken)
  {
    var customers = await _unitOfWork.Customers.GetCustomers().ToListAsync(cancellationToken);

    if (customers.Count == 0)
      return Errors.CustomerErrors.NotFound;

    return customers.ConvertAll(customer => new CustomerResponse(
        customer.Id,
        customer.Name));
  }
}
