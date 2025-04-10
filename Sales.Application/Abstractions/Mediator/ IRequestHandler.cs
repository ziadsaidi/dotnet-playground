using ErrorOr;

namespace Sales.Application.Abstractions.Mediator;

public interface IRequestHandler<in TRequest, TResult>
    where TRequest : IRequest<TResult>
{
  Task<ErrorOr<TResult>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
