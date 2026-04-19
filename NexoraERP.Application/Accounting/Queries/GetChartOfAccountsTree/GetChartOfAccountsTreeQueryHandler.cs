using MediatR;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Application.Accounting.Models;

namespace NexoraERP.Application.Accounting.Queries.GetChartOfAccountsTree;

public sealed class GetChartOfAccountsTreeQueryHandler(IChartAccountRepository chartAccounts)
    : IRequestHandler<GetChartOfAccountsTreeQuery, IReadOnlyList<ChartAccountNodeDto>>
{
    public async Task<IReadOnlyList<ChartAccountNodeDto>> Handle(
        GetChartOfAccountsTreeQuery request,
        CancellationToken cancellationToken)
    {
        var flat = await chartAccounts.GetAllOrderedForHierarchyAsync(cancellationToken);
        var nodes = flat.ToDictionary(
            a => a.Id,
            a => new ChartAccountNodeDto
            {
                Id = a.Id,
                ParentAccountId = a.ParentAccountId,
                Code = a.Code,
                Name = a.Name,
                AccountType = a.AccountType,
                NormalBalance = a.NormalBalance,
                IsPostingAccount = a.IsPostingAccount,
                IsActive = a.IsActive,
                SortOrder = a.SortOrder
            });

        var roots = new List<ChartAccountNodeDto>();
        foreach (var a in flat)
        {
            var node = nodes[a.Id];
            if (a.ParentAccountId is null)
            {
                roots.Add(node);
                continue;
            }

            if (nodes.TryGetValue(a.ParentAccountId.Value, out var parent))
                parent.Children.Add(node);
        }

        return roots;
    }
}
