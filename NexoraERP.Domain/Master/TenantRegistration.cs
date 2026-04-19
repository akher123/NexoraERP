using NexoraERP.Domain.Enums;

namespace NexoraERP.Domain.Master;

/// <summary>Catalog row stored in the master database (one row per tenant / database).</summary>
public sealed class TenantRegistration
{
    public Guid Id { get; set; }

    /// <summary>Display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Unique key used for subdomain routing (e.g. <c>acme</c> in acme.contoso.com).
    /// </summary>
    public string SubdomainKey { get; set; } = string.Empty;

    /// <summary>ADO.NET / EF Core connection string for this tenant's database.</summary>
    public string ConnectionString { get; set; } = string.Empty;

    public TenantStatus Status { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}
