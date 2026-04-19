using Microsoft.EntityFrameworkCore;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Domain.Accounting;
using NexoraERP.Infrastructure.Persistence.Tenant;

namespace NexoraERP.Infrastructure.Persistence.Repositories;

public sealed class ChartAccountRepository(ITenantAppDbContextFactory dbFactory) : IChartAccountRepository
{
    public async Task AddAsync(ChartAccount account, CancellationToken cancellationToken = default)
    {
        await using var db = dbFactory.CreateDbContext();
        db.ChartAccounts.Add(account);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default)
    {
        await using var db = dbFactory.CreateDbContext();
        var c = code.Trim();
        return await db.ChartAccounts.AnyAsync(x => x.Code == c, cancellationToken);
    }

    public async Task<ChartAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = dbFactory.CreateDbContext();
        return await db.ChartAccounts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ChartAccount>> GetAllOrderedForHierarchyAsync(
        CancellationToken cancellationToken = default)
    {
        await using var db = dbFactory.CreateDbContext();
        return await db.ChartAccounts.AsNoTracking()
            .OrderBy(x => x.HierarchyPath)
            .ThenBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);
    }
}
