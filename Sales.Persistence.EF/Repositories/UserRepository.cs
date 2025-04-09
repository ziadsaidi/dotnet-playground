
using Microsoft.EntityFrameworkCore;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Persistence.EF.Data.Configuration;

namespace Sales.Persistence.EF.Repositories;

using EF = Microsoft.EntityFrameworkCore.EF;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
  private readonly AppDbContext _dbContext = dbContext;
  public Task AddAsync(User user, CancellationToken cancellationToken)
  {
    return _dbContext.Users.AddAsync(user, cancellationToken).AsTask();
  }

  public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
  {
    return _dbContext.Users.FirstOrDefaultAsync(u => EF.Functions.Like(u.Email, email), cancellationToken: cancellationToken);
  }

  public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
  {
    return _dbContext.Users.FirstOrDefaultAsync(u => EF.Functions.Like(u.Username, username), cancellationToken: cancellationToken);
  }
}
