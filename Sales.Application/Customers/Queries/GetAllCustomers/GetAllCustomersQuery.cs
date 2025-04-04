
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Common;

namespace Sales.Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQuery(IUnitOfWork unitOfWork) : IGetAllCustomerQuery
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<ErrorOr<List<CustomerResponse>>> ExecuteAsync(CancellationToken cancellationToken)
  {
    var customers = await _unitOfWork.Customers.GetCustomers().ToListAsync(cancellationToken);

    if (customers.Count == 0)
      return Errors.CustomerErrors.NotFound;

    return customers.ConvertAll(customer => new CustomerResponse(
        customer.Id,
        customer.Name));
  }
}
