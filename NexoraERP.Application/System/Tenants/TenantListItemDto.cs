using NexoraERP.Domain.Enums;

namespace NexoraERP.Application.System.Tenants;

public sealed record TenantListItemDto(
    Guid Id,
    string Name,
    string SubdomainKey,
    TenantStatus Status,
    DateTimeOffset CreatedAtUtc);
