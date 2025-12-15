using AuthOtpSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthOtpSample.Infrastructure.Database.Configurations;

public sealed class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(x => x.Code).HasMaxLength(16).IsRequired();
        builder.HasIndex(x => new { x.UserId, x.Type, x.ExpirationDate });

        builder.HasOne(x => x.User)
            .WithMany(x => x.Otps)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}