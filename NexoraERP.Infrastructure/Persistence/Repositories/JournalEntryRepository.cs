using Microsoft.EntityFrameworkCore;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Application.Accounting.Models;
using NexoraERP.Domain.Accounting;
using NexoraERP.Infrastructure.Persistence.Tenant;

namespace NexoraERP.Infrastructure.Persistence.Repositories;

public sealed class JournalEntryRepository(ITenantAppDbContextFactory dbFactory) : IJournalEntryRepository
{
    public async Task AddAsync(JournalEntry entry, CancellationToken cancellationToken = default)
    {
        await using var db = dbFactory.CreateDbContext();
        db.JournalEntries.Add(entry);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<JournalEntry?> GetByIdWithLinesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = dbFactory.CreateDbContext();
        return await db.JournalEntries
            .Include(e => e.Lines.OrderBy(l => l.LineNumber))
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<JournalEntrySummary> Items, int TotalCount)> ListSummariesAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 200) pageSize = 200;

        await using var db = dbFactory.CreateDbContext();
        var query = db.JournalEntries.AsNoTracking().OrderByDescending(e => e.EntrySequence);
        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new JournalEntrySummary(
                e.Id,
                e.EntrySequence,
                e.EntryDate,
                e.BaseCurrencyCode,
                e.Status,
                e.Lines.Count))
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
