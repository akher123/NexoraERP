using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NexoraERP.Infrastructure.Persistence.Tenant;

public sealed class TenantAppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<TenantAppDbContext>
{
    public TenantAppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TenantAppDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=NexoraERP_Tenant_Sample;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new TenantAppDbContext(options);
    }
}
