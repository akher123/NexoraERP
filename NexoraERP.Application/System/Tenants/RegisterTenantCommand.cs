using MediatR;

namespace NexoraERP.Application.System.Tenants;

public sealed record RegisterTenantCommand(
    string Name,
    string SubdomainKey,
    string ConnectionString) : IRequest<Guid>;
