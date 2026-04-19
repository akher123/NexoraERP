namespace NexoraERP.Application.Abstractions;

/// <summary>Applies EF Core migrations to tenant databases (code-first, per-tenant connection string).</summary>
public interface ITenantDatabaseMigrator
{
    /// <summary>Runs pending migrations for a specific tenant database.</summary>
    Task MigrateTenantDatabaseAsync(string connectionString, CancellationToken cancellationToken = default);

    /// <summary>Runs migrations for every active tenant listed in the master catalog.</summary>
    Task MigrateAllActiveTenantDatabasesAsync(CancellationToken cancellationToken = default);
}
