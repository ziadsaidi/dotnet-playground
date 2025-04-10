using ErrorOr;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Common.Mapping;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Errors;

namespace Sales.Application.Customers.Queries.GetById;

public sealed class GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper<Customer, CustomerResponse> mapper) : IRequestHandler<GetCustomerByIdQuery, CustomerResponse?>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IMapper<Customer, CustomerResponse> _mapper = mapper;

  public async Task<ErrorOr<CustomerResponse?>> HandleAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken)
  {
    Customer? customer = await _unitOfWork.Customers.GetByIdAsync(query.Id, cancellationToken);

    return customer is null
        ? DomainErrors.CustomerErrors.NotFound
        : _mapper.Map(customer);
  }
}
