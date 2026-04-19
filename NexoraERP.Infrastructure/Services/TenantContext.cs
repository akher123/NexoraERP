using NexoraERP.Application.Abstractions;

namespace NexoraERP.Infrastructure.Services;

/// <summary>Scoped per request; populated by tenant resolution middleware.</summary>
public sealed class TenantContext : ITenantContext
{
    public Guid? TenantId { get; private set; }
    public string? TenantName { get; private set; }
    public string? ConnectionString { get; private set; }
    public bool IsResolved => TenantId.HasValue;

    public void SetTenant(Guid tenantId, string tenantName, string connectionString)
    {
        TenantId = tenantId;
        TenantName = tenantName;
        ConnectionString = connectionString;
    }

    public void Clear()
    {
        TenantId = null;
        TenantName = null;
        ConnectionString = null;
    }
}
