using AuthOtpSample.Application.Abstractions.Common;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Domain.Common;
using AuthOtpSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthOtpSample.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUser currentUser)
    : DbContext(options), IAppDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Otp> Otps => Set<Otp>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.HasQueryFilter(x => !x.IsDeleted);

            b.Property(x => x.Email).HasMaxLength(256).IsRequired();
            b.HasIndex(x => x.Email).IsUnique().HasFilter("[IsDeleted] = 0");

            b.Property(x => x.DateOfBirth)
             .HasConversion(
                 v => v.HasValue ? v.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                 v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null)
             .HasColumnType("date");

            b.HasOne(x => x.Notification)
             .WithOne(x => x.User)
             .HasForeignKey<Notification>(x => x.UserId);
        });

        modelBuilder.Entity<Notification>(b =>
        {
            b.HasQueryFilter(x => !x.IsDeleted);
            b.HasIndex(x => x.UserId).IsUnique().HasFilter("[IsDeleted] = 0");
        });

        modelBuilder.Entity<Otp>(b =>
        {
            b.HasQueryFilter(x => !x.IsDeleted);
            b.Property(x => x.Code).HasMaxLength(16).IsRequired();
            b.HasIndex(x => new { x.UserId, x.Type, x.ExpirationDate });
        });
    }

    public override int SaveChanges()
    {
        ApplyAuditAndSoftDelete();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditAndSoftDelete();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditAndSoftDelete()
    {
        var now = DateTime.UtcNow;
        var actor = string.IsNullOrWhiteSpace(currentUser.UserId.ToString()) ? "system" : currentUser.UserId.ToString();

        foreach (var entry in ChangeTracker.Entries<AbstractAuditEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                Entry(entry.Entity).Property(x => x.CreatedAt).IsModified = false;
                Entry(entry.Entity).Property(x => x.CreatedBy).IsModified = false;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Entity.IsDeleted = false;
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = actor;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = actor;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = now;
                entry.Entity.DeletedBy = actor;
            }

            if (entry.State == EntityState.Modified && entry.Entity.IsDeleted && entry.Entity.DeletedAt is null)
            {
                entry.Entity.DeletedAt = now;
                entry.Entity.DeletedBy = actor;
            }
        }
    }
}
