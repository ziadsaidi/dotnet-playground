
using Application.Customers.Common.Responses;
using Application.Interfaces;
using Domain;
using Domain.Common.Errors;
using Domain.Entities;
using ErrorOr;

namespace Application.Customers.Queries.GetCustomerByIdQuery;
public class GetCustomerByIdQuery(IApplicationDbContext Context) : IGetCustomerByIdQuery
{
  public async Task<ErrorOr<CustomerResponse?>> ExecuteAsync(Guid id, CancellationToken cancellationToken)
  {
    var customer = await Context.Customers.FindAsync([id, cancellationToken], cancellationToken);

    if (customer is null) return Errors.Customer.NotFound;
    return new CustomerResponse(customer.Id, customer.Name);
  }
}
