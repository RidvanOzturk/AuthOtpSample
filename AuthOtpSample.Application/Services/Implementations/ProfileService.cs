using AuthOtpSample.Application.Abstractions.Common;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.DTOs;
using AuthOtpSample.Application.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AuthOtpSample.Application.Services.Implementations;

public class ProfileService(IAppDbContext appDbContext, ICurrentUser currentUser) : IProfileService
{
    public async Task<ProfileDto?> GetAsync(CancellationToken cancellationToken)
    {
        var userId = GetUserIdOrThrow();

        var result = await appDbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new ProfileDto(
                u.Name,
                u.Surname,
                u.DateOfBirth
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task UpdateAsync(UpdateProfileDto command, CancellationToken cancellationToken)
    {
        var userId = GetUserIdOrThrow();

        var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.Name = command.Name;
        user.Surname = command.Surname;
        user.DateOfBirth = command.DateOfBirth;

        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ClearAsync(CancellationToken cancellationToken)
    {
        var userId = GetUserIdOrThrow();

        var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
        {
            return;
        }

        user.Name = null;
        user.Surname = null;
        user.DateOfBirth = null;

        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    private int GetUserIdOrThrow()
    {
        return currentUser.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
    }
}
