
using ErrorOr;

namespace Sales.Application.Abstractions.Mediator;

public interface IAppMediator
{
  Task<ErrorOr<TResult>> Send<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default);
}
