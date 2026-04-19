using NexoraERP.Domain.Accounting.Enums;
using NexoraERP.Domain.Accounting.Exceptions;
using NexoraERP.Domain.Common;

namespace NexoraERP.Domain.Accounting;

/// <summary>
/// Journal header + lines (aggregate). Base currency is the tenant functional currency used for double-entry validation.
/// </summary>
public sealed class JournalEntry : EntityBase
{
    /// <summary>Monotonic per-tenant sequence (assigned by database).</summary>
    public int EntrySequence { get; private set; }

    public DateOnly EntryDate { get; private set; }

    /// <summary>Tenant functional / reporting currency (ISO 4217).</summary>
    public string BaseCurrencyCode { get; private set; } = "USD";

    public JournalEntryStatus Status { get; private set; } = JournalEntryStatus.Draft;

    public string? Reference { get; private set; }

    public string? Memo { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? PostedAtUtc { get; private set; }

    public List<JournalLine> Lines { get; private set; } = new();

    private JournalEntry()
    {
    }

    public static JournalEntry CreateDraft(
        DateOnly entryDate,
        string baseCurrencyCode,
        string? reference,
        string? memo)
    {
        if (string.IsNullOrWhiteSpace(baseCurrencyCode) || baseCurrencyCode.Length != 3)
            throw new ArgumentException("Base currency must be a 3-letter ISO code.", nameof(baseCurrencyCode));

        return new JournalEntry
        {
            EntryDate = entryDate,
            BaseCurrencyCode = baseCurrencyCode.Trim().ToUpperInvariant(),
            Reference = string.IsNullOrWhiteSpace(reference) ? null : reference.Trim(),
            Memo = string.IsNullOrWhiteSpace(memo) ? null : memo.Trim(),
            Status = JournalEntryStatus.Draft,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    public void AddLine(JournalLine line)
    {
        ArgumentNullException.ThrowIfNull(line);
        if (line.JournalEntryId != Id)
            throw new InvalidOperationException("Line does not belong to this entry.");
        if (Status != JournalEntryStatus.Draft)
            throw new InvalidOperationException("Lines can only be added while the journal is in draft.");

        Lines.Add(line);
    }

    /// <summary>Validates Σ base debits = Σ base credits (double-entry in functional currency).</summary>
    public void EnsureBalancedInBaseCurrency()
    {
        if (Lines.Count < 2)
            throw new AccountingRuleViolationException("A posted journal must contain at least two lines.");

        var debit = Lines.Sum(l => l.BaseDebitAmount);
        var credit = Lines.Sum(l => l.BaseCreditAmount);

        if (debit != credit)
            throw new AccountingRuleViolationException(
                $"Double-entry is not balanced in base currency ({BaseCurrencyCode}): debits {debit} ≠ credits {credit}.");
    }

    public void Post()
    {
        if (Status != JournalEntryStatus.Draft)
            throw new InvalidOperationException("Only draft entries can be posted.");

        EnsureBalancedInBaseCurrency();
        Status = JournalEntryStatus.Posted;
        PostedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Void()
    {
        if (Status != JournalEntryStatus.Posted)
            throw new InvalidOperationException("Only posted entries can be voided.");

        Status = JournalEntryStatus.Voided;
    }
}
