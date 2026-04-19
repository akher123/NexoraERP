using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexoraERP.Application.Abstractions;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Infrastructure.Configuration;
using NexoraERP.Infrastructure.Exceptions;
using NexoraERP.Infrastructure.Persistence.Master;
using NexoraERP.Domain.Identity;
using NexoraERP.Infrastructure.Persistence.Repositories;
using NexoraERP.Infrastructure.Persistence.Tenant;
using NexoraERP.Infrastructure.Security;
using NexoraERP.Infrastructure.Services;

namespace NexoraERP.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MultiTenancyOptions>(configuration.GetSection(MultiTenancyOptions.SectionName));

        var masterConnection = configuration.GetConnectionString("Master")
            ?? throw new InvalidOperationException("Connection string 'Master' is not configured.");

        services.AddDbContext<MasterDbContext>(options =>
            options.UseSqlServer(masterConnection));

        services.AddScoped<TenantContext>();
        services.AddScoped<ITenantContext>(sp => sp.GetRequiredService<TenantContext>());
        services.AddScoped<ITenantRegistryReader, TenantRegistryReader>();
        services.AddScoped<ITenantAppDbContextFactory, TenantAppDbContextFactory>();
        services.AddScoped<ITenantDatabaseMigrator, TenantDatabaseMigrator>();

        services.AddSingleton<IPasswordHasher<TenantUser>, PasswordHasher<TenantUser>>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddScoped<INotesRepository, NotesRepository>();
        services.AddScoped<IChartAccountRepository, ChartAccountRepository>();
        services.AddScoped<IJournalEntryRepository, JournalEntryRepository>();
        services.AddScoped<ITenantUserRepository, TenantUserRepository>();
        services.AddScoped<IMasterTenantRepository, MasterTenantRepository>();

        services.AddExceptionHandler<AccountingRuleExceptionHandler>();

        return services;
    }
}
