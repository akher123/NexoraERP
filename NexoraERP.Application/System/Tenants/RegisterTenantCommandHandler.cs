using MediatR;
using NexoraERP.Application.Abstractions;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Domain.Enums;
using NexoraERP.Domain.Master;

namespace NexoraERP.Application.System.Tenants;

public sealed class RegisterTenantCommandHandler(
    IMasterTenantRepository masterTenants,
    ITenantDatabaseMigrator tenantMigrator)
    : IRequestHandler<RegisterTenantCommand, Guid>
{
    public async Task<Guid> Handle(RegisterTenantCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name is required.", nameof(command));
        if (string.IsNullOrWhiteSpace(command.SubdomainKey))
            throw new ArgumentException("Subdomain key is required.", nameof(command));
        if (string.IsNullOrWhiteSpace(command.ConnectionString))
            throw new ArgumentException("Connection string is required.", nameof(command));

        var key = command.SubdomainKey.Trim().ToLowerInvariant();
        if (await masterTenants.SubdomainExistsAsync(key, cancellationToken))
            throw new InvalidOperationException($"Subdomain '{key}' is already registered.");

        var tenant = new TenantRegistration
        {
            Id = Guid.NewGuid(),
            Name = command.Name.Trim(),
            SubdomainKey = key,
            ConnectionString = command.ConnectionString.Trim(),
            Status = TenantStatus.Active,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        await masterTenants.InsertAsync(tenant, cancellationToken);
        await masterTenants.SaveChangesAsync(cancellationToken);

        await tenantMigrator.MigrateTenantDatabaseAsync(tenant.ConnectionString, cancellationToken);

        return tenant.Id;
    }
}
