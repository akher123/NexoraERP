using MediatR;
using NexoraERP.Application.Abstractions;
using NexoraERP.Application.TenantInfo;

namespace NexoraERP.Application.TenantInfo.Queries.GetCurrentTenant;

public sealed class GetCurrentTenantQueryHandler(ITenantContext tenantContext)
    : IRequestHandler<GetCurrentTenantQuery, TenantInfoDto?>
{
    public Task<TenantInfoDto?> Handle(GetCurrentTenantQuery request, CancellationToken cancellationToken)
    {
        if (!tenantContext.IsResolved)
            return Task.FromResult<TenantInfoDto?>(null);

        return Task.FromResult<TenantInfoDto?>(
            new TenantInfoDto(tenantContext.TenantId!.Value, tenantContext.TenantName!));
    }
}
