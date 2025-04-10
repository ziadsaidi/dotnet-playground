using Sales.Application.Common.Mapping;
using Sales.Application.Customers.Common.Responses;
using Sales.Domain.Entities;

namespace Sales.Application.Customers.Common.Mapping;

public class CustomerMapper : IMapper<Customer, CustomerResponse>
{
  public CustomerResponse Map(Customer source)
  {
    return new CustomerResponse(
        source.Id,
        source.User?.FullName!,
        source.Address,
        source.User?.Phone!,
        source?.User.Email!
    );
  }
}