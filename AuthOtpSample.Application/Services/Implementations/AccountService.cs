using AuthOtpSample.Application.Abstractions.Notifications;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.Abstractions.Security;
using AuthOtpSample.Application.DTOs;
using AuthOtpSample.Application.Services.Contracts;
using AuthOtpSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthOtpSample.Application.Services.Implementations;

public class AccountService(
    IAppDbContext appDbContext,
    IPasswordHasher hasher,
    IEmailSender emailSender,
    ISmsSender smsSender
) : IAccountService
{
    public async Task RegisterAsync(RegisterDto request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            HashPassword = hasher.Hash(request.Password),
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

        await appDbContext.SaveChangesAsync(cancellationToken);

        await emailSender.SendAsync(
            toEmail: user.Email,
            subject: "Confirm your account",
            body: $"Your OTP code: {otpCode}",
            ct: cancellationToken
        );
    }

    public async Task ConfirmOtpAsync(ConfirmOtpDto request, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users
        .FirstAsync(x => x.Email == request.Email, cancellationToken);

        var otp = await appDbContext.Otps.FirstOrDefaultAsync(x =>
            x.UserId == user.Id &&
            x.Type == OtpType.Confirmation &&
            x.Code == request.Otp &&
            x.ExpirationDate > DateTime.UtcNow, cancellationToken);

        if (otp is null)
        {
            throw new InvalidOperationException("Invalid otp");
        }

        appDbContext.Otps.Remove(otp);
        user.IsActive = true;

        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto command, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.Email == command.Email, cancellationToken);
        if (user is null)
        {
            return;
        }

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

    public async Task ConfirmPasswordOtpAsync(ConfirmPasswordOtpDto command, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users.FirstAsync(x => x.Email == command.Email, cancellationToken);

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
        var user = await appDbContext.Users
         .FirstAsync(x => x.Id == userId, cancellationToken);

        appDbContext.Users.Remove(user);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    private static string GenerateOtp6()
    {
        var n = RandomNumberGenerator.GetInt32(0, 1_000_000);
        return n.ToString("D6");
    }
}
