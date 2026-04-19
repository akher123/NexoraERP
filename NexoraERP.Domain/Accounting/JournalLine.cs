using NexoraERP.Domain.Accounting.Exceptions;

namespace NexoraERP.Domain.Accounting;

/// <summary>
/// Journal line: GL account only, debit XOR credit in transaction currency, converted to tenant base currency.
/// </summary>
public sealed class JournalLine
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid JournalEntryId { get; private set; }

    public int LineNumber { get; private set; }

    /// <summary>Posting (leaf) GL account.</summary>
    public Guid AccountId { get; private set; }

    /// <summary>Debit in <see cref="CurrencyCode"/>; zero if this line is a credit.</summary>
    public decimal TransactionDebit { get; private set; }

    /// <summary>Credit in <see cref="CurrencyCode"/>; zero if this line is a debit.</summary>
    public decimal TransactionCredit { get; private set; }

    /// <summary>ISO 4217 currency of the transaction amounts.</summary>
    public string CurrencyCode { get; private set; } = string.Empty;

    /// <summary>
    /// Multiplier: base currency amount = transaction amount × rate (per 1 unit of transaction currency).
    /// </summary>
    public decimal ExchangeRateToBase { get; private set; }

    public decimal BaseDebitAmount { get; private set; }

    public decimal BaseCreditAmount { get; private set; }

    private JournalLine()
    {
    }

    public static JournalLine Create(
        Guid journalEntryId,
        int lineNumber,
        Guid accountId,
        decimal transactionDebit,
        decimal transactionCredit,
        string currencyCode,
        decimal exchangeRateToBase)
    {
        if (lineNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(lineNumber));
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            throw new ArgumentException("A 3-letter ISO currency code is required.", nameof(currencyCode));
        if (exchangeRateToBase <= 0)
            throw new ArgumentOutOfRangeException(nameof(exchangeRateToBase), "Exchange rate must be positive.");

        if (transactionDebit < 0 || transactionCredit < 0)
            throw new ArgumentException("Debit and credit amounts cannot be negative.");

        if (transactionDebit > 0 && transactionCredit > 0)
            throw new AccountingRuleViolationException("A journal line cannot have both debit and credit in the same row.");

        if (transactionDebit == 0 && transactionCredit == 0)
            throw new AccountingRuleViolationException("A journal line must have either a debit or a credit amount.");

        var cc = currencyCode.Trim().ToUpperInvariant();
        var baseDebit = transactionDebit * exchangeRateToBase;
        var baseCredit = transactionCredit * exchangeRateToBase;

        return new JournalLine
        {
            JournalEntryId = journalEntryId,
            LineNumber = lineNumber,
            AccountId = accountId,
            TransactionDebit = transactionDebit,
            TransactionCredit = transactionCredit,
            CurrencyCode = cc,
            ExchangeRateToBase = exchangeRateToBase,
            BaseDebitAmount = baseDebit,
            BaseCreditAmount = baseCredit
        };
    }
}
