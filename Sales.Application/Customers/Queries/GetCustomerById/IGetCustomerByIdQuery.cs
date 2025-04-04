using ErrorOr;
using Sales.Application.Customers.Common.Responses;

namespace Sales.Application.Customers.Queries.GetCustomerById
{
  public interface IGetCustomerByIdQuery
  {
    public Task<ErrorOr<CustomerResponse?>> ExecuteAsync(Guid id, CancellationToken cancellationToken);
  }
}
