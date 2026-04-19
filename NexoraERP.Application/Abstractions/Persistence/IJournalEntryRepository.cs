using NexoraERP.Application.Accounting.Models;
using NexoraERP.Domain.Accounting;

namespace NexoraERP.Application.Abstractions.Persistence;

public interface IJournalEntryRepository
{
    Task AddAsync(JournalEntry entry, CancellationToken cancellationToken = default);

    Task<JournalEntry?> GetByIdWithLinesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<JournalEntrySummary> Items, int TotalCount)> ListSummariesAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
