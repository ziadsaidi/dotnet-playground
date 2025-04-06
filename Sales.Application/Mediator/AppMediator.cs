using ErrorOr;
using Microsoft.Extensions.DependencyInjection;

namespace Sales.Application.Mediator;

public class AppMediator(IServiceProvider serviceProvider) : IAppMediator
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public async Task<ErrorOr<TResult>> Send<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
  {
    // Find the appropriate handler for the request using Dependency Injection
    var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
    var handler = _serviceProvider.GetRequiredService(handlerType);

    // Use reflection to invoke the handler's Handle method
    var handleMethod = handlerType.GetMethod("HandleAsync");
    return await (Task<ErrorOr<TResult>>)handleMethod.Invoke(handler, [request, cancellationToken]);
  }
}
