namespace NexoraERP.Domain.Accounting.ValueObjects;

/// <summary>Monetary amount with ISO 4217 currency (tenant-scoped reporting).</summary>
public readonly record struct Money(decimal Amount, string CurrencyCode)
{
    public static Money Of(decimal amount, string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currencyCode));
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
        return new Money(amount, currencyCode.Trim().ToUpperInvariant());
    }
}
