using MediatR;
using NexoraERP.Application.Accounting.Models;

namespace NexoraERP.Application.Accounting.Queries.ListJournalEntries;

public sealed record ListJournalEntriesQuery(int Page = 1, int PageSize = 20)
    : IRequest<(IReadOnlyList<JournalEntrySummary> Items, int TotalCount)>;
