using AuthOtpSample.Domain.Common;

namespace AuthOtpSample.Domain.Entities;

public class User : AbstractAuditEntity
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;

    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    public bool IsActive { get; set; }

    public Notification? Notification { get; set; }
    public ICollection<Otp> Otps { get; set; } = new List<Otp>();
}
