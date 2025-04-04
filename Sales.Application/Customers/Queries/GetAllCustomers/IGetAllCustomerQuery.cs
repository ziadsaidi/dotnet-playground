using ErrorOr;
using Sales.Application.Customers.Common.Responses;
namespace Sales.Application.Customers.Queries.GetAllCustomers;
public interface IGetAllCustomerQuery
{
  Task<ErrorOr<List<CustomerResponse>>> ExecuteAsync(CancellationToken cancellationToken);
}
