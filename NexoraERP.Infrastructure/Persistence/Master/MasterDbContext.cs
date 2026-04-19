using Microsoft.EntityFrameworkCore;
using NexoraERP.Domain.Master;

namespace NexoraERP.Infrastructure.Persistence.Master;

public sealed class MasterDbContext : DbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options)
        : base(options)
    {
    }

    public DbSet<TenantRegistration> Tenants => Set<TenantRegistration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TenantRegistration>(b =>
        {
            b.ToTable("Tenants");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(256).IsRequired();
            b.HasIndex(x => x.SubdomainKey).IsUnique();
            b.Property(x => x.SubdomainKey).HasMaxLength(128).IsRequired();
            b.Property(x => x.ConnectionString).IsRequired();
            b.Property(x => x.Status).HasConversion<int>();
            b.Property(x => x.CreatedAtUtc).IsRequired();
        });
    }
}
