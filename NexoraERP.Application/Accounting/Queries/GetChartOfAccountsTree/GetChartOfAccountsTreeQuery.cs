using MediatR;
using NexoraERP.Application.Accounting.Models;

namespace NexoraERP.Application.Accounting.Queries.GetChartOfAccountsTree;

public sealed record GetChartOfAccountsTreeQuery : IRequest<IReadOnlyList<ChartAccountNodeDto>>;
