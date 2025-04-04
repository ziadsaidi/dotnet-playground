using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Common.Responses;
using Application.Interfaces;
using Domain.Common.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQuery(IApplicationDbContext Context) : IGetAllCustomerQuery
{
  public async Task<ErrorOr<List<CustomerResponse>>> ExecuteAsync(CancellationToken cancellationToken)
  {
    var customers = await Context.Customers.ToListAsync(cancellationToken);
    if (customers is { Count: 0 })
      return Errors.Customer.NotFound;

    return customers.ConvertAll(customer => new CustomerResponse(
       customer.Id,
       customer.Name));
  }
}
