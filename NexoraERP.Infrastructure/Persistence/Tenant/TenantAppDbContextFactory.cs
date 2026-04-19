using Microsoft.EntityFrameworkCore;
using NexoraERP.Application.Abstractions;
using NexoraERP.Shared.Exceptions;

namespace NexoraERP.Infrastructure.Persistence.Tenant;

public sealed class TenantAppDbContextFactory(ITenantContext tenantContext) : ITenantAppDbContextFactory
{
    public TenantAppDbContext CreateDbContext()
    {
        if (!tenantContext.IsResolved || string.IsNullOrWhiteSpace(tenantContext.ConnectionString))
            throw new TenantResolutionException("Tenant is not resolved; cannot create tenant database context.");

        var options = new DbContextOptionsBuilder<TenantAppDbContext>()
            .UseSqlServer(tenantContext.ConnectionString)
            .Options;

        return new TenantAppDbContext(options);
    }
}
