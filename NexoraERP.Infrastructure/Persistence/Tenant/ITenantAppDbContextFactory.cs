namespace NexoraERP.Infrastructure.Persistence.Tenant;

/// <summary>Builds a <see cref="TenantAppDbContext"/> using the resolved tenant connection string.</summary>
public interface ITenantAppDbContextFactory
{
    TenantAppDbContext CreateDbContext();
}
