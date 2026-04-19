using MediatR;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Application.Accounting.Models;

namespace NexoraERP.Application.Accounting.Queries.ListJournalEntries;

public sealed class ListJournalEntriesQueryHandler(IJournalEntryRepository journals)
    : IRequestHandler<ListJournalEntriesQuery, (IReadOnlyList<JournalEntrySummary> Items, int TotalCount)>
{
    public Task<(IReadOnlyList<JournalEntrySummary> Items, int TotalCount)> Handle(
        ListJournalEntriesQuery request,
        CancellationToken cancellationToken)
    {
        return journals.ListSummariesAsync(request.Page, request.PageSize, cancellationToken);
    }
}
