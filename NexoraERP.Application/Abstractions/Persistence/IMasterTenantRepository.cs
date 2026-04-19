using NexoraERP.Domain.Master;

namespace NexoraERP.Application.Abstractions.Persistence;

/// <summary>Master catalog persistence (single database, not tenant-scoped).</summary>
public interface IMasterTenantRepository
{
    Task<IReadOnlyList<TenantRegistration>> ListAllAsync(CancellationToken cancellationToken = default);

    Task<TenantRegistration?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> SubdomainExistsAsync(string subdomainKey, CancellationToken cancellationToken = default);

    /// <summary>Tracks a new tenant; call <see cref="SaveChangesAsync"/> after provisioning.</summary>
    Task InsertAsync(TenantRegistration tenant, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
