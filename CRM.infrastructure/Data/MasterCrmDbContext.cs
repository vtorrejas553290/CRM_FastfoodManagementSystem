using CRM.domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.infrastructure.Data;

public class MasterCrmDbContext : IdentityDbContext
{
    public MasterCrmDbContext(DbContextOptions<MasterCrmDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<CompanyDatabase> CompanyDatabases => Set<CompanyDatabase>();

    public DbSet<Plan> Plans => Set<Plan>();

    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Company>(entity =>
        {
            entity.HasKey(x => x.CompanyId);

            entity.Property(x => x.CompanyCode)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.CompanyName)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(x => x.CompanyCode)
                .IsUnique();

            entity.HasMany(x => x.CompanyDatabases)
                .WithOne(x => x.Company)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<CompanyDatabase>(entity =>
        {
            entity.HasKey(x => x.CompanyDatabaseId);

            entity.Property(x => x.ServerName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.DatabaseName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.CredentialKey)
                .HasMaxLength(100)
                .IsRequired();
        });

        // ---------- NEW ----------

        builder.Entity<Plan>(entity =>
        {
            entity.HasKey(x => x.PlanId);

            entity.Property(x => x.PlanCode)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.PlanName)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.PlanCode)
                .IsUnique();
        });

        builder.Entity<Subscription>(entity =>
        {
            entity.HasKey(x => x.SubscriptionId);

            entity.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Plan)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new { x.CompanyId, x.IsActive });
        });
    }
}