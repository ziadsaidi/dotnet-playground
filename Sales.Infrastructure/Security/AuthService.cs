using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Sales.Application.Interfaces;

namespace Sales.Infrastructure.Security
{
  public class AuthService : IAuthService
  {
    private readonly string _secret;
    private readonly IUserRepository _userRepository;

    public AuthService(string secret, IUserRepository userRepository)
    {
      _secret = secret;
      _userRepository = userRepository;
    }

    public string GenerateToken(Guid userId, string userEmail)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret))
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
          issuer: "YourIssuer",
          audience: "YourAudience",
          claims: claims,
          expires: DateTime.UtcNow.AddHours(1),
          signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
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
