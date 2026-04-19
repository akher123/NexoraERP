using NexoraERP.Domain.Common;

namespace NexoraERP.Domain.Identity;

/// <summary>Application user stored in the tenant database (isolated per tenant).</summary>
public sealed class TenantUser : EntityBase
{
    public string UserName { get; private set; } = string.Empty;

    public string NormalizedUserName { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAtUtc { get; private set; }

    private TenantUser()
    {
    }

    public static TenantUser Create(string userName, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("Username is required.", nameof(userName));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        var name = userName.Trim();
        return new TenantUser
        {
            UserName = name,
            NormalizedUserName = name.ToUpperInvariant(),
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    /// <summary>Replaces the stored password hash (e.g. after computing with the application password hasher).</summary>
    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        PasswordHash = passwordHash;
    }
}
