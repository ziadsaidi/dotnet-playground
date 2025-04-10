using ErrorOr;

namespace Sales.Application.Interfaces;

public interface ITokenExtractorService
{
  public ErrorOr<string> ExtractToken();
}
