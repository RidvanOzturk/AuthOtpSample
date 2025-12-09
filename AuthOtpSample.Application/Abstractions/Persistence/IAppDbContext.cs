using AuthOtpSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthOtpSample.Application.Abstractions.Persistence;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Otp> Otps { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}