using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Api.Extensions;

public static class ErrorOrExtensions
{
  public static IActionResult ToActionResult(this List<Error> errors)
  {
    if (errors is null || errors.Count == 0)
    {
      return new ObjectResult(new ProblemDetails
      {
        Status = StatusCodes.Status500InternalServerError,
        Title = "An unexpected error occurred",
        Detail = "Unknown error."
      })
      {
        StatusCode = StatusCodes.Status500InternalServerError
      };
    }

    var first = errors[0];

    return first.Type switch
    {
      ErrorType.Validation => new BadRequestObjectResult(errors),
      ErrorType.Unauthorized => new UnauthorizedObjectResult(errors),
      ErrorType.Conflict => new ConflictObjectResult(errors),
      ErrorType.NotFound => new NotFoundObjectResult(errors),
      _ => new ObjectResult(new ProblemDetails
      {
        Status = StatusCodes.Status500InternalServerError,
        Title = "An unexpected error occurred",
        Detail = string.Join(", ", errors.Select(e => e.Description))
      })
      {
        StatusCode = StatusCodes.Status500InternalServerError
      }
    };
  }
}
