using NexoraERP.Domain.Common;

namespace NexoraERP.Domain.TenantData;

/// <summary>Example aggregate stored in each tenant's own database.</summary>
public sealed class TenantNote : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}
