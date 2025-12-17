using AuthOtpSample.Application.DTOs;

namespace AuthOtpSample.Application.Services.Contracts;

public interface IProfileService
{
    Task<ProfileDto?> GetAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UpdateProfileDto request, CancellationToken cancellationToken);
    Task ClearAsync(CancellationToken cancellationToken);
}
