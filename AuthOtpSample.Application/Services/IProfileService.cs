using AuthOtpSample.Application.Features.Profile;

namespace AuthOtpSample.Application.Services;

public interface IProfileService
{
    Task<ProfileDto?> GetAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UpdateProfileCommand command, CancellationToken cancellationToken);
    Task ClearAsync(CancellationToken cancellationToken);
}
