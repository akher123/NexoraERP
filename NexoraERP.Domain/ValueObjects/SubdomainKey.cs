using System.Text.RegularExpressions;

namespace NexoraERP.Domain.ValueObjects;

/// <summary>Normalized subdomain identifier used for host-based routing (e.g. acme → acme.contoso.com).</summary>
public readonly partial record struct SubdomainKey
{
    private const int MaxLength = 128;

    private SubdomainKey(string value) => Value = value;

    public string Value { get; }

    public static SubdomainKey Create(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("Subdomain key is required.", nameof(raw));

        var v = raw.Trim().ToLowerInvariant();
        if (v.Length > MaxLength)
            throw new ArgumentOutOfRangeException(nameof(raw), "Subdomain key is too long.");

        if (!SlugRegex().IsMatch(v))
            throw new ArgumentException("Subdomain key must be alphanumeric with optional hyphens.", nameof(raw));

        return new SubdomainKey(v);
    }

    public static bool TryCreate(string? raw, out SubdomainKey key)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            key = default;
            return false;
        }

        try
        {
            key = Create(raw);
            return true;
        }
        catch (ArgumentException)
        {
            key = default;
            return false;
        }
    }

    [GeneratedRegex("^[a-z0-9]+(-[a-z0-9]+)*$", RegexOptions.CultureInvariant)]
    private static partial Regex SlugRegex();
}
