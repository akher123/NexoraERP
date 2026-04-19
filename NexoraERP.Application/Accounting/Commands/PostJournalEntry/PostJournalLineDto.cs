namespace NexoraERP.Application.Accounting.Commands.PostJournalEntry;

public sealed record PostJournalLineDto(
    Guid AccountId,
    decimal TransactionDebit,
    decimal TransactionCredit,
    string CurrencyCode,
    decimal ExchangeRateToBase);
