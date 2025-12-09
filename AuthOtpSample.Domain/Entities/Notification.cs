using AuthOtpSample.Domain.Common;

namespace AuthOtpSample.Domain.Entities;

public class Notification : AbstractAuditEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public bool IsSmsNotificationEnabled { get; set; }
    public bool IsEmailNotificationEnabled { get; set; }

    public User User { get; set; } = default!;
}
