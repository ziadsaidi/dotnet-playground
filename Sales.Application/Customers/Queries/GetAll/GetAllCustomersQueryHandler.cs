using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Common.Mapping;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Application.Customers.Queries.GetAll;

public sealed class GetAllCustomersQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper<Customer, CustomerResponse> mapper)
    : IRequestHandler<GetAllCustomersQuery, List<CustomerResponse>>
{
  public async Task<ErrorOr<List<CustomerResponse>>> HandleAsync(GetAllCustomersQuery request, CancellationToken cancellationToken) =>
    (await unitOfWork.Customers.GetCustomers().ToListAsync(cancellationToken)).ConvertAll(mapper.Map);
}
