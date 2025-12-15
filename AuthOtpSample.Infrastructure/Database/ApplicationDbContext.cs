using AuthOtpSample.Application.Abstractions.Common;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Domain.Common;
using AuthOtpSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthOtpSample.Infrastructure.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUser currentUser) : DbContext(options), IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
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
        var createdBy = currentUser.UserId.HasValue
            ? currentUser.UserId.Value.ToString()
            : "system";

        foreach (var entry in ChangeTracker.Entries<AbstractAuditEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = createdBy;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = createdBy;
                Entry(entry.Entity).Property(x => x.CreatedAt).IsModified = false;
                Entry(entry.Entity).Property(x => x.CreatedBy).IsModified = false;
                Entry(entry.Entity).Property(x => x.DeletedAt).IsModified = false;
                Entry(entry.Entity).Property(x => x.DeletedBy).IsModified = false;
                Entry(entry.Entity).Property(x => x.IsDeleted).IsModified = false;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = now;
                entry.Entity.DeletedBy = createdBy;
                Entry(entry.Entity).Property(x => x.UpdatedAt).IsModified = false;
                Entry(entry.Entity).Property(x => x.UpdatedBy).IsModified = false;
                Entry(entry.Entity).Property(x => x.CreatedAt).IsModified = false;
                Entry(entry.Entity).Property(x => x.CreatedBy).IsModified = false;
            }
        }
    }
}