using Sales.Domain.Entities;

namespace Sales.Application.Interfaces;

public interface IUserRepository
{
  Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
  Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
  Task AddAsync(User user, CancellationToken cancellationToken);
}
