using NexoraERP.Domain.Accounting;

namespace NexoraERP.Application.Abstractions.Persistence;

public interface IChartAccountRepository
{
    Task<ChartAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ChartAccount>> GetAllOrderedForHierarchyAsync(CancellationToken cancellationToken = default);

    Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default);

    Task AddAsync(ChartAccount account, CancellationToken cancellationToken = default);
}
