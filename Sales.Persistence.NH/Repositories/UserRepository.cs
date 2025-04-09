
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Sales.Persistence.NH.Repositories;

public class UserRepository(ISession session) : IUserRepository
{
  private readonly ISession _session = session;
  public Task AddAsync(User user, CancellationToken cancellationToken)
  {
    return _session.SaveAsync(user, cancellationToken);
  }

  public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
  {
    return _session.Query<User>()
          .Where(u => u.Email == email)
          .FirstOrDefaultAsync(cancellationToken: cancellationToken)!;
  }

  public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
  {
    return _session.Query<User>()
          .Where(u => u.Username == username)
          .FirstOrDefaultAsync(cancellationToken: cancellationToken)!;
  }
}
