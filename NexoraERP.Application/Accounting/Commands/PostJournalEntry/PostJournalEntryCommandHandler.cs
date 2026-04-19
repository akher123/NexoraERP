using MediatR;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Domain.Accounting;
using NexoraERP.Domain.Accounting.Exceptions;

namespace NexoraERP.Application.Accounting.Commands.PostJournalEntry;

public sealed class PostJournalEntryCommandHandler(
    IJournalEntryRepository journalEntries,
    IChartAccountRepository chartAccounts)
    : IRequestHandler<PostJournalEntryCommand, Guid>
{
    public async Task<Guid> Handle(PostJournalEntryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command.Lines);

        var entry = JournalEntry.CreateDraft(
            command.EntryDate,
            command.BaseCurrencyCode,
            command.Reference,
            command.Memo);

        var lineNumber = 1;
        foreach (var dto in command.Lines)
        {
            var account = await chartAccounts.GetByIdAsync(dto.AccountId, cancellationToken);
            if (account is null)
                throw new AccountingRuleViolationException($"GL account {dto.AccountId} was not found.");
            if (!account.IsPostingAccount)
                throw new AccountingRuleViolationException($"Account '{account.Code}' is not a posting account.");

            var line = JournalLine.Create(
                entry.Id,
                lineNumber++,
                dto.AccountId,
                dto.TransactionDebit,
                dto.TransactionCredit,
                dto.CurrencyCode,
                dto.ExchangeRateToBase);

            entry.AddLine(line);
        }

        entry.Post();
        await journalEntries.AddAsync(entry, cancellationToken);
        return entry.Id;
    }
}
