namespace NexoraERP.Domain.Accounting.Exceptions;

public sealed class AccountingRuleViolationException : Exception
{
    public AccountingRuleViolationException(string message)
        : base(message)
    {
    }
}
