using Microsoft.EntityFrameworkCore;
using NexoraERP.Application.Abstractions;
using NexoraERP.Domain.Enums;
using NexoraERP.Domain.Master;
using NexoraERP.Infrastructure.Persistence.Master;

namespace NexoraERP.Infrastructure.Services;

public sealed class TenantRegistryReader(MasterDbContext masterDb) : ITenantRegistryReader
{
    public Task<TenantRegistration?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return masterDb.Tenants.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);
    }

    public Task<TenantRegistration?> GetBySubdomainKeyAsync(string subdomainKey, CancellationToken cancellationToken = default)
    {
        var key = subdomainKey.Trim().ToLowerInvariant();
        return masterDb.Tenants.AsNoTracking()
            .FirstOrDefaultAsync(t => t.SubdomainKey == key, cancellationToken);
    }

    public async Task<IReadOnlyList<TenantRegistration>> ListActiveAsync(CancellationToken cancellationToken = default)
    {
        var list = await masterDb.Tenants.AsNoTracking()
            .Where(t => t.Status == TenantStatus.Active)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);

        return list;
    }
}
