
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Common.Mapping;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Errors;

namespace Sales.Application.Customers.Queries.GetAll;

public sealed class GetAllCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper<Customer, CustomerResponse> mapper) : IRequestHandler<GetAllCustomersQuery, List<CustomerResponse>>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IMapper<Customer, CustomerResponse> _mapper = mapper;
  public async Task<ErrorOr<List<CustomerResponse>>> HandleAsync(GetAllCustomersQuery request, CancellationToken cancellationToken)
  {
    var customers = await _unitOfWork.Customers.GetCustomers().ToListAsync(cancellationToken);

    if (customers.Count == 0)
    {
      return DomainErrors.CustomerErrors.NotFound;
    }

    return customers.ConvertAll(_mapper.Map);
  }
}
