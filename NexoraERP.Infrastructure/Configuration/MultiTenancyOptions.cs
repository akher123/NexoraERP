namespace NexoraERP.Infrastructure.Configuration;

public sealed class MultiTenancyOptions
{
    public const string SectionName = "MultiTenancy";

    /// <summary>Host suffix used with <see cref="NexoraERP.Shared.MultiTenancy.SubdomainParser"/> (e.g. contoso.com or localhost).</summary>
    public string BaseDomain { get; set; } = "localhost";

    public bool EnableSubdomainResolution { get; set; } = true;

    /// <summary>Path prefixes that skip tenant resolution (e.g. health, OpenAPI).</summary>
    public string[] ExcludedPathPrefixes { get; set; } = ["/health", "/swagger", "/openapi"];
}
