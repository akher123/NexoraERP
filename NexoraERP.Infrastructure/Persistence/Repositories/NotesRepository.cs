using Microsoft.EntityFrameworkCore;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Domain.TenantData;
using NexoraERP.Infrastructure.Persistence.Tenant;

namespace NexoraERP.Infrastructure.Persistence.Repositories;

public sealed class NotesRepository(ITenantAppDbContextFactory dbFactory) : INotesRepository
{
    public async Task<IReadOnlyList<TenantNote>> ListOrderedByCreatedDescendingAsync(
        CancellationToken cancellationToken = default)
    {
        await using var db = dbFactory.CreateDbContext();
        return await db.Notes.AsNoTracking()
            .OrderByDescending(n => n.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}
