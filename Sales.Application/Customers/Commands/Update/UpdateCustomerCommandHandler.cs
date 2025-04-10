using ErrorOr;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Common;
using Sales.Application.Interfaces;
using Sales.Domain.Errors;

namespace Sales.Application.Customers.Commands.Update;

public class UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<UpdateCustomerCommand, Unit>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IUserContext _userContext = userContext;
  public async Task<ErrorOr<Unit>> HandleAsync(UpdateCustomerCommand request, CancellationToken cancellationToken)
  {
    var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id, cancellationToken);
    if (customer is null)
    {
      return DomainErrors.CustomerErrors.NotFound;
    }

    var userIdResult = _userContext.GetAuthenticatedUserId();

    if (userIdResult.IsError)
    {
      return userIdResult.Errors;
    }

    customer.UpdateAddress(request.Address!);

    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Unit.Value;
  }
}
