using ErrorOr;

namespace Sales.Application.Interfaces;

public interface IUserContext
{
  ErrorOr<Guid> GetAuthenticatedUserId();
  bool IsAuthenticated { get; }
}