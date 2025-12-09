using AuthOtpSample.Domain.Common;

namespace AuthOtpSample.Domain.Entities;

public class Otp : AbstractAuditEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }

    public OtpType Type { get; set; }

    public User User { get; set; } = default!;
}
