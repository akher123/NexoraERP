namespace NexoraERP.Application.Abstractions;

public sealed record JwtAccessTokenResult(string AccessToken, DateTimeOffset ExpiresAtUtc, int ExpiresInSeconds);

public interface IJwtTokenService
{
    JwtAccessTokenResult CreateAccessToken(Guid userId, string userName, Guid tenantId);
}
