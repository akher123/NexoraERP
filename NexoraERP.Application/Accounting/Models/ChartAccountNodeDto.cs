using NexoraERP.Domain.Accounting.Enums;

namespace NexoraERP.Application.Accounting.Models;

public sealed class ChartAccountNodeDto
{
    public required Guid Id { get; init; }
    public Guid? ParentAccountId { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public AccountType AccountType { get; init; }
    public AccountNormalBalance NormalBalance { get; init; }
    public bool IsPostingAccount { get; init; }
    public bool IsActive { get; init; }
    public int SortOrder { get; init; }
    public List<ChartAccountNodeDto> Children { get; } = new();
}
