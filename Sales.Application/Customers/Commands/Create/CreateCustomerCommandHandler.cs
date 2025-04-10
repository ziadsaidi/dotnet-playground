using ErrorOr;
using Sales.Application.Customers.Common.Responses;
using Sales.Domain.Entities;
using Sales.Application.Interfaces;
using Sales.Application.Common.Mapping;
using Sales.Application.Abstractions.Mediator;

namespace Sales.Application.Customers.Commands.Create;

public sealed class CreateCustomerCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper<Customer, CustomerResponse> customermapper,
    IUserContext userContext) : IRequestHandler<CreateCustomerCommand, CustomerResponse?>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IUserContext _userContext = userContext;
  private readonly IMapper<Customer, CustomerResponse> _customerMapper = customermapper;

  public async Task<ErrorOr<CustomerResponse?>> HandleAsync(CreateCustomerCommand model, CancellationToken cancellationToken = default)
  {
    ErrorOr<Guid> userIdResult = _userContext.GetAuthenticatedUserId();

    if (userIdResult.IsError)
    {
      return userIdResult.Errors;
    }

    Customer customer = Customer.Create(userIdResult.Value, model.Address);

    await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
    _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

    Customer? customerWithUser = await _unitOfWork.Customers
        .GetByIdWithUserAsync(customer.Id, cancellationToken);

    return _customerMapper.Map(customerWithUser!);
  }
}
