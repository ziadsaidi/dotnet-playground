using ErrorOr;
using Microsoft.Extensions.DependencyInjection;

namespace Sales.Application.Abstractions.Mediator;

public class AppMediator(IServiceProvider serviceProvider) : IAppMediator
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public async Task<ErrorOr<TResponse>> Send<TResponse>(
      IRequest<TResponse> request,
      CancellationToken cancellationToken = default)
  {
    var requestType = request.GetType();
    var responseType = typeof(TResponse);

    // Get handler for this request
    var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
    var handler = _serviceProvider.GetService(handlerType);

    if (handler == null)
    {
      return Error.Unexpected($"No handler registered for {requestType.Name}");
    }

    // Get behaviors for this request
    var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
    var behaviors = _serviceProvider.GetServices(pipelineType).ToList();

    // Create a pipeline of behaviors
    return await ExecutePipeline(request, handler, behaviors, cancellationToken);
  }

  private static async Task<ErrorOr<TResponse>> ExecutePipeline<TResponse>(
      IRequest<TResponse> request,
      object handler,
      IList<object> behaviors,
      CancellationToken cancellationToken)
  {
    RequestHandler<TResponse> handlerDelegate = async () =>
    {
      var method = handler.GetType().GetMethod("HandleAsync");

      if (method == null)
      {
        return Error.Unexpected($"Handler for {request.GetType().Name} does not implement HandleAsync.");
      }

      var result = method.Invoke(handler, [request, cancellationToken]);

      if (result is Task<ErrorOr<TResponse>> task)
      {
        return await task;
      }

      return Error.Unexpected("Invalid handler response type.");
    };

    // Chain the behaviors together
    foreach (var behavior in behaviors.Reverse())
    {
      var currentDelegate = handlerDelegate;
      var behaviorMethod = behavior.GetType().GetMethod("HandleAsync");

      if (behaviorMethod == null)
      {
        continue;
      }

      handlerDelegate = async () =>
      {
        var result = behaviorMethod.Invoke(
                  behavior,
                  [request, currentDelegate, cancellationToken]);

        if (result is Task<ErrorOr<TResponse>> task)
        {
          return await task;
        }

        return Error.Unexpected("Invalid behavior response type.");
      };
    }
    return await handlerDelegate();
  }
}