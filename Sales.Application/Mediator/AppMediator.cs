using ErrorOr;

namespace Sales.Application.Mediator;

public class AppMediator(IServiceProvider serviceProvider) : IAppMediator
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public async Task<ErrorOr<TResult>> Send<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
  {
    var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
    var handler = _serviceProvider.GetService(handlerType);

    if (handler is null)
    {
      return Error.Unexpected($"No handler registered for {request.GetType().Name}");
    }

    var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResult>, TResult>.HandleAsync));

    if (handleMethod is null)
    {
      return Error.Unexpected($"Handler for {request.GetType().Name} does not implement HandleAsync correctly.");
    }

    // Appel de manière sécurisée avec cast
    var result = handleMethod.Invoke(handler, [request, cancellationToken]);

    return result is Task<ErrorOr<TResult>> task
        ? await task
        : Error.Unexpected("Invalid handler response type.");
  }
}
