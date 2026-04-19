using MediatR;
using NexoraERP.Application.Abstractions.Persistence;

namespace NexoraERP.Application.System.Tenants;

public sealed class ListTenantsQueryHandler(IMasterTenantRepository masterTenants)
    : IRequestHandler<ListTenantsQuery, IReadOnlyList<TenantListItemDto>>
{
    public async Task<IReadOnlyList<TenantListItemDto>> Handle(
        ListTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var rows = await masterTenants.ListAllAsync(cancellationToken);
        return rows.Select(t => new TenantListItemDto(t.Id, t.Name, t.SubdomainKey, t.Status, t.CreatedAtUtc))
            .ToList();
    }
}
