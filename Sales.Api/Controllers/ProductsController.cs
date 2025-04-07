using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using Sales.Application.Mediator;
using Sales.Application.Products.Commands.Create;
using Sales.Application.Products.Queries.GetAll;
using Sales.Application.Products.Queries.GetById;
namespace Sales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/problem+json")]
public sealed class ProductsController(
    IAppMediator mediator
    ) : ControllerBase
{
  private readonly IAppMediator _mediator = mediator;

  /// <summary>
  /// Creates a new customer
  /// </summary>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> Create([FromBody] CreateProductCommand model)
  {
    var result = await _mediator.Send(model, CancellationToken.None);

    return result.Match(
        response => CreatedAtAction(
            nameof(GetById),
            new { id = response?.Id },
            response),
        errors => errors?.First().Type switch
        {
          ErrorType.Validation => BadRequest(errors),
          ErrorType.Conflict => Conflict(errors),
          ErrorType.NotFound => NotFound(errors),
          _ => StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                      Status = StatusCodes.Status500InternalServerError,
                      Title = "An unexpected error occurred",
                      Detail = string.Join(", ", errors!.Select(e => e.Description))
                    })
        });
  }

  /// <summary>
  /// Retrieves all customers
  /// </summary>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAll()
  {
    var result = await _mediator.Send(new GetAllProductsQuery(), CancellationToken.None);

    return result.Match(
        Ok,
        errors => StatusCode(StatusCodes.Status500InternalServerError,
            new ProblemDetails
            {
              Status = StatusCodes.Status500InternalServerError,
              Title = "An unexpected error occurred",
              Detail = string.Join(", ", errors!.Select(e => e.Description))
            }));
  }

  /// <summary>
  /// Retrieves a customer by ID
  /// </summary>
  /// <param name="id">The ID of the customer</param>
  /// <returns>The customer data or error details</returns>
  /// <response code="200">Returns the customer details</response>
  /// <response code="404">If the customer is not found</response>
  /// <response code="500">If an unexpected error occurs</response>
  [HttpGet("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetById(Guid id)
  {
    var result = await _mediator.Send(new GetProductByIdQuery(id), CancellationToken.None);

    return result.Match(
        Ok,
        errors => errors?.First().Type switch
        {
          ErrorType.NotFound => NotFound(errors),
          _ => StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                      Status = StatusCodes.Status500InternalServerError,
                      Title = "An unexpected error occurred",
                      Detail = string.Join(", ", errors!.Select(e => e.Description))
                    })
        });
  }
}
