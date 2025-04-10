namespace Sales.Infrastructure.Identity.Options;

public record JwtSettings(string Secret, string Issuer, string Audience, int ExpiryMinutes = 60);
