using MediatR;
using NexoraERP.Application.TenantInfo;

namespace NexoraERP.Application.TenantInfo.Queries.GetCurrentTenant;

public sealed record GetCurrentTenantQuery : IRequest<TenantInfoDto?>;
