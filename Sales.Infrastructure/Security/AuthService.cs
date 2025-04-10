using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Sales.Application.Interfaces;
using Sales.Infrastructure.Identity.Options;

namespace Sales.Infrastructure.Security
{
  public class AuthService : IAuthService
  {
    private readonly IUserRepository _userRepository;

    private readonly JwtSettings _jwtSettings;

    public AuthService(JwtSettings jwtSettings, IUserRepository userRepository)
    {
      _userRepository = userRepository;
      _jwtSettings = jwtSettings;
    }

    public string GenerateToken(Guid userId, string userEmail)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
      {
        KeyId = null
      };

      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

      var token = new JwtSecurityToken(
          issuer: _jwtSettings.Issuer,
          audience: _jwtSettings.Audience,
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
          signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      try
      {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        var tokenValidationParameters = new TokenValidationParameters
        {
          ValidateLifetime = true,
          ValidAudience = _jwtSettings.Audience,
          ValidIssuer = _jwtSettings.Issuer,
          IssuerSigningKey = new SymmetricSecurityKey(key),
        };

        var principal = tokenHandler.ValidateToken(token!, tokenValidationParameters, out SecurityToken? validatedToken);

        if (validatedToken is JwtSecurityToken jwtToken)
        {
          return principal;
        }

        return null;
      }
      catch (Exception)
      {
        return null;
      }
    }

    public async Task<bool> ValidateUserAsync(string email, string password, CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
      if (user is null) return false;

      // Verify the password hash using BCrypt
      return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
  }
}
