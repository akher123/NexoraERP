using Microsoft.EntityFrameworkCore;
using NexoraERP.Domain.Accounting;
using NexoraERP.Domain.Accounting.Enums;
using NexoraERP.Domain.Identity;
using NexoraERP.Domain.TenantData;

namespace NexoraERP.Infrastructure.Persistence.Tenant;

public sealed class TenantAppDbContext : DbContext
{
    public TenantAppDbContext(DbContextOptions<TenantAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TenantNote> Notes => Set<TenantNote>();

    public DbSet<ChartAccount> ChartAccounts => Set<ChartAccount>();

    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();

    public DbSet<JournalLine> JournalLines => Set<JournalLine>();

    public DbSet<TenantUser> Users => Set<TenantUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TenantNote>(b =>
        {
            b.ToTable("Notes");
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).HasMaxLength(256).IsRequired();
            b.Property(x => x.Body).HasMaxLength(4000);
            b.Property(x => x.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<ChartAccount>(b =>
        {
            b.ToTable("ChartAccounts");
            b.HasKey(x => x.Id);
            b.Property(x => x.Code).HasMaxLength(64).IsRequired();
            b.Property(x => x.Name).HasMaxLength(512).IsRequired();
            b.Property(x => x.HierarchyPath).HasMaxLength(2048).IsRequired();
            b.Property(x => x.NormalBalance).HasConversion<int>();
            b.Property(x => x.AccountType).HasConversion<int>();
            b.HasIndex(x => x.Code).IsUnique();
            b.HasIndex(x => x.ParentAccountId);
            b.HasIndex(x => x.HierarchyPath);

            b.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<JournalEntry>(b =>
        {
            b.ToTable("JournalEntries");
            b.HasKey(x => x.Id);
            b.Property(x => x.EntrySequence).UseIdentityColumn();
            b.Property(x => x.EntryDate).IsRequired();
            b.Property(x => x.BaseCurrencyCode).HasMaxLength(3).IsRequired();
            b.Property(x => x.Status).HasConversion<int>();
            b.Property(x => x.Reference).HasMaxLength(128);
            b.Property(x => x.Memo).HasMaxLength(1024);
            b.Property(x => x.CreatedAtUtc).IsRequired();
            b.HasIndex(x => x.EntryDate);
            b.HasIndex(x => x.Status);
            b.HasMany(x => x.Lines)
                .WithOne()
                .HasForeignKey(l => l.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TenantUser>(b =>
        {
            b.ToTable("Users");
            b.HasKey(x => x.Id);
            b.Property(x => x.UserName).HasMaxLength(256).IsRequired();
            b.Property(x => x.NormalizedUserName).HasMaxLength(256).IsRequired();
            b.HasIndex(x => x.NormalizedUserName).IsUnique();
            b.Property(x => x.PasswordHash).IsRequired();
            b.Property(x => x.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<JournalLine>(b =>
        {
            b.ToTable("JournalLines");
            b.HasKey(x => x.Id);
            b.Property(x => x.TransactionDebit).HasPrecision(18, 4);
            b.Property(x => x.TransactionCredit).HasPrecision(18, 4);
            b.Property(x => x.ExchangeRateToBase).HasPrecision(18, 8);
            b.Property(x => x.BaseDebitAmount).HasPrecision(18, 4);
            b.Property(x => x.BaseCreditAmount).HasPrecision(18, 4);
            b.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();
            b.HasIndex(x => new { x.JournalEntryId, x.LineNumber }).IsUnique();
        });
    }
}
