using AuthOtpSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthOtpSample.Infrastructure.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique().HasFilter("[IsDeleted] = 0");

        builder.Property(x => x.DateOfBirth)
            .HasConversion(
                v => v.HasValue ? v.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null)
            .HasColumnType("date");

        builder.HasOne(x => x.Notification)
            .WithOne(x => x.User)
            .HasForeignKey<Notification>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}