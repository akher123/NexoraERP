using MediatR;

namespace NexoraERP.Application.System.Tenants;

public sealed record ListTenantsQuery : IRequest<IReadOnlyList<TenantListItemDto>>;
