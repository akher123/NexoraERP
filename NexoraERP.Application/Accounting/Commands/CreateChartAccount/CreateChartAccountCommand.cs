using MediatR;
using NexoraERP.Domain.Accounting.Enums;

namespace NexoraERP.Application.Accounting.Commands.CreateChartAccount;

public sealed record CreateChartAccountCommand(
    Guid? ParentAccountId,
    string Code,
    string Name,
    AccountType AccountType,
    AccountNormalBalance NormalBalance,
    bool IsPostingAccount,
    int SortOrder) : IRequest<Guid>;
