using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class User : IEntity
{
  /// <summary>
  /// Needed for NHibernate
  /// </summary>
  public User() { }
  private User(Guid id, string username, string email, string passwordHash)
  {
    Id = id;
    Username = username;
    Email = email;
    PasswordHash = passwordHash;
  }

  public virtual Guid Id { get; protected set; }
  public virtual string Username { get; protected set; } = null!;
  public virtual string Email { get; protected set; } = null!;
  public virtual string PasswordHash { get; protected set; } = null!;

  public static User Create(string username, string email, string passwordHash)
  {
    return new User(Guid.CreateVersion7(), username, email, passwordHash);
  }

  public virtual void UpdatePassword(string newPasswordHash)
  {
    PasswordHash = newPasswordHash;
  }
}
