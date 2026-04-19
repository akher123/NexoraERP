using NexoraERP.Domain.Accounting.Enums;
using NexoraERP.Domain.Common;

namespace NexoraERP.Domain.Accounting;

/// <summary>
/// Hierarchical GL chart of accounts. Summary accounts roll up posting accounts.
/// </summary>
public sealed class ChartAccount : EntityBase
{
    public Guid? ParentAccountId { get; private set; }

    public ChartAccount? Parent { get; private set; }

    /// <summary>Materialized path for subtree queries (e.g. <c>/guid1/guid2/</c>).</summary>
    public string HierarchyPath { get; private set; } = "/";

    public int Depth { get; private set; }

    public int SortOrder { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public AccountType AccountType { get; private set; }

    public AccountNormalBalance NormalBalance { get; private set; }

    /// <summary>Only posting accounts may appear on journal lines.</summary>
    public bool IsPostingAccount { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? ModifiedAtUtc { get; private set; }

    public ICollection<ChartAccount> Children { get; private set; } = new List<ChartAccount>();

    private ChartAccount()
    {
    }

    public static ChartAccount CreateRoot(
        string code,
        string name,
        AccountType accountType,
        AccountNormalBalance normalBalance,
        bool isPostingAccount,
        int sortOrder)
    {
        ValidateCodeName(code, name);
        var id = Guid.NewGuid();
        return new ChartAccount
        {
            Id = id,
            ParentAccountId = null,
            Depth = 0,
            HierarchyPath = $"/{id:N}/",
            Code = code.Trim(),
            Name = name.Trim(),
            AccountType = accountType,
            NormalBalance = normalBalance,
            IsPostingAccount = isPostingAccount,
            SortOrder = sortOrder,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    public static ChartAccount CreateChild(
        ChartAccount parent,
        string code,
        string name,
        AccountType accountType,
        AccountNormalBalance normalBalance,
        bool isPostingAccount,
        int sortOrder)
    {
        ArgumentNullException.ThrowIfNull(parent);
        ValidateCodeName(code, name);

        var id = Guid.NewGuid();
        return new ChartAccount
        {
            Id = id,
            ParentAccountId = parent.Id,
            Depth = parent.Depth + 1,
            HierarchyPath = $"{parent.HierarchyPath}{id:N}/",
            Code = code.Trim(),
            Name = name.Trim(),
            AccountType = accountType,
            NormalBalance = normalBalance,
            IsPostingAccount = isPostingAccount,
            SortOrder = sortOrder,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        Name = name.Trim();
        ModifiedAtUtc = DateTimeOffset.UtcNow;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
        ModifiedAtUtc = DateTimeOffset.UtcNow;
    }

    private static void ValidateCodeName(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Account code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Account name is required.", nameof(name));
    }
}
