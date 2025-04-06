using ErrorOr;

namespace Sales.Application.Mediator;

public interface IRequestHandler<in TRequest, TResult>
    where TRequest : IRequest<TResult>
{
  Task<ErrorOr<TResult>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
