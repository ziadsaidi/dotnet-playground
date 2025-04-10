using ErrorOr;
using FluentValidation;
using Sales.Application.Abstractions.Mediator;

namespace Sales.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

  public async Task<ErrorOr<TResponse>> HandleAsync(
      TRequest request,
      RequestHandler<TResponse> nextStep,
      CancellationToken cancellationToken)
  {
    if (!_validators.Any())
    {
      return await nextStep();
    }

    var validationFailures = (await Task.WhenAll(
        _validators.Select(validator =>
            validator.ValidateAsync(request, cancellationToken))))
        .SelectMany(result => result.Errors)
        .Where(failure => failure != null)
        .ToList();

    if (validationFailures.Count == 0)
    {
      return await nextStep();
    }

    return validationFailures
        .ConvertAll(error => Error.Validation(
            error.PropertyName,
            error.ErrorMessage));
  }
}