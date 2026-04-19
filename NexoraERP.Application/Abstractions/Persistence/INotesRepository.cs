using NexoraERP.Domain.TenantData;

namespace NexoraERP.Application.Abstractions.Persistence;

public interface INotesRepository
{
    Task<IReadOnlyList<TenantNote>> ListOrderedByCreatedDescendingAsync(CancellationToken cancellationToken = default);
}
