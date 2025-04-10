using ErrorOr;
using System.Security.Claims;
using Sales.Application.Interfaces;

namespace Sales.Infrastructure.Identity;

public class UserContext(
    ITokenExtractorService tokenExtractorService,
    IAuthService authService) : IUserContext
{
  private readonly ITokenExtractorService _tokenExtractorService = tokenExtractorService;
  private readonly IAuthService _authService = authService;

  public bool IsAuthenticated
  {
    get
    {
      var result = GetAuthenticatedUserId();
      return !result.IsError;
    }
  }

  public ErrorOr<Guid> GetAuthenticatedUserId()
  {
    var tokenResult = _tokenExtractorService.ExtractToken();

    if (tokenResult.IsError)
    {
      return tokenResult.Errors;
    }

    var principal = _authService.GetPrincipalFromToken(tokenResult.Value!);
    if (principal is null)
    {
      return Error.Unauthorized(description: "Invalid or expired token.");
    }

    var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
    {
      return Error.Unauthorized(description: "User ID not found in token claims.");
    }

    if (Guid.TryParse(userIdClaim.Value, out Guid userId))
    {
      return userId;
    }

    return Error.Unauthorized(description: "Invalid user ID format in token.");
  }
}