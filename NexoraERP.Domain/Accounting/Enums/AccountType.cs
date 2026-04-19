namespace NexoraERP.Domain.Accounting.Enums;

/// <summary>High-level classification for reporting and roll-up (IFRS-style).</summary>
public enum AccountType
{
    Asset = 1,
    Liability = 2,
    Equity = 3,
    Revenue = 4,
    Expense = 5,
    Other = 99
}
