namespace NexoraERP.Application.Abstractions;

/// <summary>Per-request tenant identity and connection information (database-per-tenant).</summary>
public interface ITenantContext
{
    Guid? TenantId { get; }
    string? TenantName { get; }
    string? ConnectionString { get; }
    bool IsResolved { get; }
}
