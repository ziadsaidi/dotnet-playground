
using ErrorOr;

namespace Sales.Application.Mediator;

public interface IAppMediator
{
  Task<ErrorOr<TResult>> Send<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default);
}
