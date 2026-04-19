using NexoraERP.Domain.Master;

namespace NexoraERP.Application.Abstractions;

/// <summary>Reads tenant metadata from the master catalog.</summary>
public interface ITenantRegistryReader
{
    Task<TenantRegistration?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default);

    Task<TenantRegistration?> GetBySubdomainKeyAsync(string subdomainKey, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TenantRegistration>> ListActiveAsync(CancellationToken cancellationToken = default);
}
