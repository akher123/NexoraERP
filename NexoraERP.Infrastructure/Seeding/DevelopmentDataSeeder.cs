using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NexoraERP.Application.Abstractions;
using NexoraERP.Domain.Enums;
using NexoraERP.Domain.Identity;
using NexoraERP.Domain.Master;
using NexoraERP.Infrastructure.Persistence.Master;
using NexoraERP.Infrastructure.Persistence.Tenant;

namespace NexoraERP.Infrastructure.Seeding;

/// <summary>Applies migrations and optional dev-only catalog + tenant DB bootstrap.</summary>
public static class DevelopmentDataSeeder
{
    public static async Task ApplyAsync(IHost app, IConfiguration configuration, IHostEnvironment env)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var sp = scope.ServiceProvider;
        var master = sp.GetRequiredService<MasterDbContext>();
        await master.Database.MigrateAsync();

        if (!env.IsDevelopment())
            return;

        var sampleTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var tenantCs = configuration["SampleTenant:ConnectionString"];
        if (string.IsNullOrWhiteSpace(tenantCs))
            return;

        if (!await master.Tenants.AnyAsync(t => t.Id == sampleTenantId))
        {
            master.Tenants.Add(new TenantRegistration
            {
                Id = sampleTenantId,
                Name = "Sample Tenant",
                SubdomainKey = "sample",
                ConnectionString = tenantCs,
                Status = TenantStatus.Active,
                CreatedAtUtc = DateTimeOffset.UtcNow
            });
            await master.SaveChangesAsync();
        }

        var migrator = sp.GetRequiredService<ITenantDatabaseMigrator>();
        await migrator.MigrateTenantDatabaseAsync(tenantCs);

        await SeedSampleTenantAdminUserAsync(sp, tenantCs);
    }

    private static async Task SeedSampleTenantAdminUserAsync(IServiceProvider sp, string tenantConnectionString)
    {
        var options = new DbContextOptionsBuilder<TenantAppDbContext>()
            .UseSqlServer(tenantConnectionString)
            .Options;

        await using var tenantDb = new TenantAppDbContext(options);
        if (await tenantDb.Users.AnyAsync(u => u.NormalizedUserName == "ADMIN"))
            return;

        var hasher = sp.GetRequiredService<IPasswordHasher<TenantUser>>();
        var admin = TenantUser.Create("admin", "TEMP-HASH-REPLACE");
        var hash = hasher.HashPassword(admin, "Admin@123");
        admin.SetPasswordHash(hash);

        tenantDb.Users.Add(admin);
        await tenantDb.SaveChangesAsync();
    }
}
