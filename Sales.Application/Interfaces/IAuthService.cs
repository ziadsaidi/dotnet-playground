namespace Sales.Application.Interfaces;

public interface IAuthService
{
  string GenerateToken(Guid userId, string userEmail);
  Task<bool> ValidateUserAsync(string email, string password, CancellationToken cancellationToken);
}
