using MediatR;
using Microsoft.AspNetCore.Identity;
using NexoraERP.Application.Abstractions;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Domain.Identity;

namespace NexoraERP.Application.Identity.Commands.Login;

public sealed class LoginCommandHandler(
    ITenantContext tenantContext,
    ITenantUserRepository users,
    IPasswordHasher<TenantUser> passwordHasher,
    IJwtTokenService jwtTokens)
    : IRequestHandler<LoginCommand, LoginResponse?>
{
    public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (!tenantContext.IsResolved || tenantContext.TenantId is null)
            return null;

        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var normalized = request.UserName.Trim().ToUpperInvariant();
        var user = await users.GetByNormalizedUserNameAsync(normalized, cancellationToken);
        if (user is null || !user.IsActive)
            return null;

        var verify = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verify == PasswordVerificationResult.Failed)
            return null;

        var tenantId = tenantContext.TenantId!.Value;
        var token = jwtTokens.CreateAccessToken(user.Id, user.UserName, tenantId);

        return new LoginResponse(
            token.AccessToken,
            "Bearer",
            token.ExpiresInSeconds,
            token.ExpiresAtUtc,
            tenantId,
            user.Id,
            user.UserName);
    }
}
