using System.Data;
using Dapper;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Persistence.Dapper.Repositories;

public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
  private readonly IDbConnection _dbConnection = dbConnection;
  public Task AddAsync(User user, CancellationToken cancellationToken)
  {
    const string sql = "INSERT INTO users(id,username,email,password_hash) Values(@Id,@Username,@Email,@PasswordHash)";
    return _dbConnection.ExecuteAsync(sql, user);
  }

  public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
  {
    const string sql = "SELECT * from users where email = @Email";
    return await _dbConnection.QueryFirstOrDefaultAsync(sql, new { Email = email });

  }

  public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
  {
    const string sql = "SELECT * from users where username = @Username";

    return await _dbConnection.QueryFirstOrDefaultAsync(sql, new { Username = username });
  }
}
