using NexoraERP.Domain.Identity;

namespace NexoraERP.Application.Abstractions.Persistence;

public interface ITenantUserRepository
{
    Task<TenantUser?> GetByNormalizedUserNameAsync(string normalizedUserName, CancellationToken cancellationToken = default);
}
