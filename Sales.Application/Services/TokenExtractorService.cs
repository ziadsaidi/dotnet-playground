using ErrorOr;
using Microsoft.AspNetCore.Http;
using Sales.Application.Interfaces;

namespace Sales.Application.Services;

public class TokenExtractorService(IHttpContextAccessor httpContextAccessor) : ITokenExtractorService
{
  private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

  public ErrorOr<string> ExtractToken()
  {
    var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

    if (string.IsNullOrEmpty(authorizationHeader))
    {
      return Error.Unauthorized("Authorization header is missing.");
    }

    var token = authorizationHeader.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
    if (string.IsNullOrEmpty(token))
    {
      return Error.Unauthorized("Token is missing or invalid.");
    }

    return token;
  }
}
