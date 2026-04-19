using NexoraERP.Domain.Accounting.Enums;

namespace NexoraERP.Application.Accounting.Models;

public sealed record JournalEntryDetailDto(
    Guid Id,
    int EntrySequence,
    DateOnly EntryDate,
    string BaseCurrencyCode,
    JournalEntryStatus Status,
    string? Reference,
    string? Memo,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? PostedAtUtc,
    IReadOnlyList<JournalLineDetailDto> Lines);

public sealed record JournalLineDetailDto(
    int LineNumber,
    Guid AccountId,
    decimal TransactionDebit,
    decimal TransactionCredit,
    string CurrencyCode,
    decimal ExchangeRateToBase,
    decimal BaseDebitAmount,
    decimal BaseCreditAmount);
