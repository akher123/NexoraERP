using Microsoft.EntityFrameworkCore;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Domain.Identity;
using NexoraERP.Infrastructure.Persistence.Tenant;

namespace NexoraERP.Infrastructure.Persistence.Repositories;

public sealed class TenantUserRepository(ITenantAppDbContextFactory dbFactory) : ITenantUserRepository
{
    public async Task<TenantUser?> GetByNormalizedUserNameAsync(
        string normalizedUserName,
        CancellationToken cancellationToken = default)
    {
        await using var db = dbFactory.CreateDbContext();
        return await db.Users.AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.NormalizedUserName == normalizedUserName && u.IsActive,
                cancellationToken);
    }
}
