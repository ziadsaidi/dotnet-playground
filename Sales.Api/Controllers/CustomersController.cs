using Microsoft.AspNetCore.Mvc;
using Sales.Application.Customers.Commands.Create;
using Sales.Application.Customers.Queries.GetAll;
using Sales.Application.Customers.Queries.GetById;
using Sales.Application.Customers.Commands.Update;
using Sales.Api.Controllers.Models;
using Sales.Application.Customers.Commands.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Sales.Application.Abstractions.Mediator;
using Sales.Api.Extensions;

namespace Sales.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
[Produces("application/problem+json")]
public sealed class CustomersController(IAppMediator mediator) : ControllerBase
{
  private readonly IAppMediator _mediator = mediator;

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> Create([FromBody] CreateCustomerCommand model)
  {
    var result = await _mediator.Send(model);
    return result.Match(
        response => CreatedAtAction(nameof(GetById), new { id = response?.Id }, response),
        errors => errors.ToActionResult()
    );
  }

  [HttpPut("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> Update(Guid id, UpdateCustomerDto dto)
  {
    var result = await _mediator.Send(new UpdateCustomerCommand(id, dto.Address));
    return result.Match(
        _ => NoContent(),
        errors => errors.ToActionResult()
    );
  }

  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAll()
  {
    var result = await _mediator.Send(new GetAllCustomersQuery());
    return result.Match(Ok, errors => errors.ToActionResult());
  }

  [HttpGet("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetById(Guid id)
  {
    var result = await _mediator.Send(new GetCustomerByIdQuery(id));
    return result.Match(Ok, errors => errors.ToActionResult());
  }

  [HttpDelete("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> Delete(Guid id)
  {
    var result = await _mediator.Send(new DeleteCustomerCommand(id));
    return result.Match(
        _ => NoContent(),
        errors => errors.ToActionResult()
    );
  }
}
