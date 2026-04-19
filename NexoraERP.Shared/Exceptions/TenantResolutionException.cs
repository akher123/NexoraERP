namespace NexoraERP.Shared.Exceptions;

public sealed class TenantResolutionException : Exception
{
    public TenantResolutionException(string message)
        : base(message)
    {
    }

    public TenantResolutionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
