using NexoraERP.Domain.Accounting.Enums;

namespace NexoraERP.Application.Accounting.Models;

public sealed record JournalEntrySummary(
    Guid Id,
    int EntrySequence,
    DateOnly EntryDate,
    string BaseCurrencyCode,
    JournalEntryStatus Status,
    int LineCount);
