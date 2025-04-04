using Application.Customers.Common.Responses;
using ErrorOr;

namespace Application.Customers.Queries.GetCustomerByIdQuery;

public interface IGetCustomerByIdQuery
{
  public Task<ErrorOr<CustomerResponse?>> ExecuteAsync(Guid id, CancellationToken cancellationToken);
}
