using Microsoft.AspNetCore.Mvc;
using Sales.Api.Extensions;
using Sales.Application.Mediator;
using Sales.Application.Users.Commands.Login;
using Sales.Application.Users.Commands.Register;

namespace Sales.Api.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public sealed class AuthController(IAppMediator mediator) : ControllerBase
  {
    private readonly IAppMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand request)
    {
      var result = await _mediator.Send(request, HttpContext.RequestAborted);

      return result.Match(
          Ok,
          errors => errors.ToActionResult()
      );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
      var result = await _mediator.Send(request, HttpContext.RequestAborted);

      return result.Match(
          Ok,
          errors => errors.ToActionResult()
      );
    }
  }
}