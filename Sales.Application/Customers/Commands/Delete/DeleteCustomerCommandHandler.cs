using ErrorOr;
using Sales.Application.Abstractions.Mediator;
using Sales.Application.Common;
using Sales.Application.Interfaces;
using Sales.Domain.Errors;

namespace Sales.Application.Customers.Commands.Delete;

public class DeleteCustomerCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCustomerCommand, Unit>
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<ErrorOr<Unit>> HandleAsync(DeleteCustomerCommand request, CancellationToken cancellationToken)
  {
    var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id, cancellationToken);

    if (customer is null)
    {
      return DomainErrors.CustomerErrors.NotFound;
    }

    await _unitOfWork.Customers.DeleteAsync(customer, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Unit.Value;
  }
}
