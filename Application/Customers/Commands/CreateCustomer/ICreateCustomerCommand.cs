
using Application.Customers.Common.Responses;
using ErrorOr;

namespace Application.Customers.Commands.CreateCustomer;

public interface ICreateCustomerCommand
{
  Task<ErrorOr<CustomerResponse?>> ExecuteAsync(CreateCustomerModel model, CancellationToken cancellationToken);
}
