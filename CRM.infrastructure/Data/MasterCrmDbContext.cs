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
    }
}