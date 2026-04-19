using Microsoft.EntityFrameworkCore;
using NexoraERP.Application.Abstractions;
using NexoraERP.Domain.Enums;
using NexoraERP.Infrastructure.Persistence.Master;
using NexoraERP.Infrastructure.Persistence.Tenant;

namespace NexoraERP.Infrastructure.Services;

public sealed class TenantDatabaseMigrator(MasterDbContext masterDb) : ITenantDatabaseMigrator
{
    public async Task MigrateTenantDatabaseAsync(string connectionString, CancellationToken cancellationToken = default)
    {
        var options = new DbContextOptionsBuilder<TenantAppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        await using var db = new TenantAppDbContext(options);
        await db.Database.MigrateAsync(cancellationToken);
    }

    public async Task MigrateAllActiveTenantDatabasesAsync(CancellationToken cancellationToken = default)
    {
        var connectionStrings = await masterDb.Tenants.AsNoTracking()
            .Where(t => t.Status == TenantStatus.Active)
            .Select(t => t.ConnectionString)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var cs in connectionStrings)
            await MigrateTenantDatabaseAsync(cs, cancellationToken);
    }
}
