using Microsoft.EntityFrameworkCore;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Domain.Master;

namespace NexoraERP.Infrastructure.Persistence.Master;

public sealed class MasterTenantRepository(MasterDbContext db) : IMasterTenantRepository
{
    public async Task<IReadOnlyList<TenantRegistration>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Tenants.AsNoTracking()
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<TenantRegistration?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return db.Tenants.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<bool> SubdomainExistsAsync(string subdomainKey, CancellationToken cancellationToken = default)
    {
        var key = subdomainKey.Trim().ToLowerInvariant();
        return db.Tenants.AnyAsync(t => t.SubdomainKey == key, cancellationToken);
    }

    public Task InsertAsync(TenantRegistration tenant, CancellationToken cancellationToken = default)
    {
        db.Tenants.Add(tenant);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return db.SaveChangesAsync(cancellationToken);
    }
}
