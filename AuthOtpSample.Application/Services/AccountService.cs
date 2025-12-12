using AuthOtpSample.Application.Abstractions.Notifications;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.Abstractions.Security;
using AuthOtpSample.Application.Features.Account.ConfirmOtp;
using AuthOtpSample.Application.Features.Account.ConfirmPasswordOtp;
using AuthOtpSample.Application.Features.Account.ForgotPassword;
using AuthOtpSample.Application.Features.Account.Register;
using AuthOtpSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthOtpSample.Application.Services;

public class AccountService(
    IAppDbContext appDbContext,
    IPasswordHasher hasher,
    IEmailSender emailSender,
    ISmsSender smsSender
) : IAccountService
{
    public async Task RegisterAsync(RegisterCommand command, CancellationToken cancellationToken)
    {
        var exists = await appDbContext.Users.AnyAsync(x => x.Email == command.Email, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            Email = command.Email,
            HashPassword = hasher.Hash(command.Password),
            IsActive = false
        };

        appDbContext.Users.Add(user);
        await appDbContext.SaveChangesAsync(cancellationToken);

        var otpCode = GenerateOtp6();
        appDbContext.Otps.Add(new Otp
        {
            UserId = user.Id,
            Code = otpCode,
            Type = OtpType.Confirmation,
            ExpirationDate = DateTime.UtcNow.AddMinutes(10)
        });

        // Default notification istersen burada da oluşturabilirsin (şimdilik sonra ekleriz)

        await appDbContext.SaveChangesAsync(cancellationToken);

        await emailSender.SendAsync(
            toEmail: user.Email,
            subject: "Confirm your account",
            body: $"Your OTP code: {otpCode}",
            ct: cancellationToken
        );
    }

    public async Task ConfirmOtpAsync(ConfirmOtpCommand command, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.Email == command.Email, cancellationToken);
        if (user is null)
        {
            return;
        }

        var otp = await appDbContext.Otps.FirstOrDefaultAsync(x =>
            x.UserId == user.Id &&
            x.Type == OtpType.Confirmation &&
            x.Code == command.Otp &&
            x.ExpirationDate > DateTime.UtcNow, cancellationToken);

        if (otp is null)
        {
            throw new InvalidOperationException("Invalid otp");
        }

        appDbContext.Otps.Remove(otp);
        user.IsActive = true;

        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.Email == command.Email, cancellationToken);
        if (user is null)
        {
            return;
        }

        // Eski ForgotPassword otp'lerini isteğe göre soft-delete edebilirsin
        var oldOtps = await appDbContext.Otps
            .Where(o => o.UserId == user.Id && o.Type == OtpType.ForgotPassword)
            .ToListAsync(cancellationToken);

        if (oldOtps.Count > 0)
        {
            appDbContext.Otps.RemoveRange(oldOtps);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }

        var otpCode = GenerateOtp6();
        appDbContext.Otps.Add(new Otp
        {
            UserId = user.Id,
            Code = otpCode,
            Type = OtpType.ForgotPassword,
            ExpirationDate = DateTime.UtcNow.AddMinutes(10)
        });

        await appDbContext.SaveChangesAsync(cancellationToken);

        // Kullanıcının notification ayarlarına göre gönder
        var notification = await appDbContext.Notifications
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.UserId == user.Id, cancellationToken);

        var message = $"Your password reset OTP code: {otpCode}";

        if (notification?.IsEmailNotificationEnabled == true)
        {
            await emailSender.SendAsync(user.Email, "Reset your password", message, cancellationToken);
        }

        if (notification?.IsSmsNotificationEnabled == true)
        {
            await smsSender.SendAsync(user.Id, message, cancellationToken);
        }
    }

    public async Task ConfirmPasswordOtpAsync(ConfirmPasswordOtpCommand command, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.Email == command.Email, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        var otp = await appDbContext.Otps.FirstOrDefaultAsync(x =>
            x.UserId == user.Id &&
            x.Type == OtpType.ForgotPassword &&
            x.Code == command.Otp &&
            x.ExpirationDate > DateTime.UtcNow, cancellationToken);

        if (otp is null)
        {
            throw new InvalidOperationException("Invalid otp");
        }

        user.HashPassword = hasher.Hash(command.NewPassword);
        appDbContext.Otps.Remove(otp);

        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAccountAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user is null)
        {
            return;
        }

        appDbContext.Users.Remove(user);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    private static string GenerateOtp6()
    {
        var n = RandomNumberGenerator.GetInt32(0, 1_000_000);
        return n.ToString("D6");
    }
}
