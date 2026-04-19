namespace NexoraERP.Shared.MultiTenancy;

/// <summary>Parses a tenant key from the request host when using subdomain routing.</summary>
public static class SubdomainParser
{
    /// <summary>
    /// Extracts the left-most subdomain label when <paramref name="host"/> ends with
    /// <c>.{baseDomain}</c>. Port is stripped from <paramref name="host"/> if present.
    /// </summary>
    /// <returns>The subdomain key, or <c>null</c> if the host matches the base domain only.</returns>
    public static string? TryGetSubdomainKey(string host, string baseDomain)
    {
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(baseDomain))
            return null;

        var hostOnly = host.Split(':')[0].TrimEnd('.');
        var domain = baseDomain.Trim().TrimEnd('.');

        if (hostOnly.Equals(domain, StringComparison.OrdinalIgnoreCase))
            return null;

        var suffix = "." + domain;
        if (!hostOnly.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            return null;

        var sub = hostOnly[..^suffix.Length];
        if (string.IsNullOrEmpty(sub))
            return null;

        // Only single-level subdomain (tenant.example.com) — adjust if you need nested subdomains.
        var firstLabel = sub.Split('.')[0];
        return string.IsNullOrEmpty(firstLabel) ? null : firstLabel;
    }
}
