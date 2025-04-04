
using ErrorOr;
using Sales.Application.Customers.Common.Responses;

namespace Sales.Application.Customers.Commands.CreateCustomer;
public interface ICreateCustomerCommand
{
  Task<ErrorOr<CustomerResponse?>> ExecuteAsync(CreateCustomerModel model, CancellationToken cancellationToken);
}
