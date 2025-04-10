using ErrorOr;

namespace Sales.Application.Abstractions.Mediator;

public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
  Task<ErrorOr<TResponse>> HandleAsync(
      TRequest request,
      RequestHandler<TResponse> nextStep,
      CancellationToken cancellationToken);
}

public delegate Task<ErrorOr<TResponse>> RequestHandler<TResponse>();