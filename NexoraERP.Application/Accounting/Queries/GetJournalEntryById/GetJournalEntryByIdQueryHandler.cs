using MediatR;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Application.Accounting.Models;

namespace NexoraERP.Application.Accounting.Queries.GetJournalEntryById;

public sealed class GetJournalEntryByIdQueryHandler(IJournalEntryRepository journals)
    : IRequestHandler<GetJournalEntryByIdQuery, JournalEntryDetailDto?>
{
    public async Task<JournalEntryDetailDto?> Handle(
        GetJournalEntryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entry = await journals.GetByIdWithLinesAsync(request.Id, cancellationToken);
        if (entry is null)
            return null;

        var lines = entry.Lines
            .OrderBy(l => l.LineNumber)
            .Select(l => new JournalLineDetailDto(
                l.LineNumber,
                l.AccountId,
                l.TransactionDebit,
                l.TransactionCredit,
                l.CurrencyCode,
                l.ExchangeRateToBase,
                l.BaseDebitAmount,
                l.BaseCreditAmount))
            .ToList();

        return new JournalEntryDetailDto(
            entry.Id,
            entry.EntrySequence,
            entry.EntryDate,
            entry.BaseCurrencyCode,
            entry.Status,
            entry.Reference,
            entry.Memo,
            entry.CreatedAtUtc,
            entry.PostedAtUtc,
            lines);
    }
}
