
using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Common.Responses;
using ErrorOr;

namespace Application.Customers.Queries.GetAllCustomers;

public interface IGetAllCustomerQuery
{
  Task<ErrorOr<List<CustomerResponse>>> ExecuteAsync(CancellationToken cancellationToken);
}
