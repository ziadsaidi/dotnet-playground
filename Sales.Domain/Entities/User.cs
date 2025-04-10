using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class User : IEntity, IAggregateRoot
{
  protected User() { }

  private User(Guid id, string username, string email, string phone, string passwordHash, Role role, string fullName)
  {
    Id = id;
    Username = username;
    Email = email;
    Phone = phone;
    PasswordHash = passwordHash;
    Role = role;
    FullName = fullName;
  }

  public virtual Guid Id { get; protected set; }
  public virtual string Username { get; protected set; } = null!;
  public virtual string FullName { get; protected set; } = null!;
  public virtual string Email { get; protected set; } = null!;
  public virtual string Phone { get; protected set; } = null!;
  public virtual string PasswordHash { get; protected set; } = null!;
  public virtual Role Role { get; protected set; }

  public virtual Customer? Customer { get; protected set; }
  public virtual Employee? Employee { get; protected set; }

  public static User Create(string username, string fullName, string email, string phone, string passwordHash, Role role)
  {
    return new User(Guid.CreateVersion7(), username, email, phone, passwordHash, role, fullName);
  }

  public virtual void UpdateProfile(string? newFullName, string? email, string? phone)
  {
    FullName = newFullName ?? FullName;
    Email = email ?? Email;
    Phone = phone ?? Phone;
  }

  public virtual void UpdatePassword(string newPasswordHash)
  {
    PasswordHash = newPasswordHash;
  }
}
