namespace NexoraERP.Application.Identity.Commands.Login;

public sealed record LoginResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    DateTimeOffset ExpiresAtUtc,
    Guid TenantId,
    Guid UserId,
    string UserName);
