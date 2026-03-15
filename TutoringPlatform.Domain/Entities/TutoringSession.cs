using TutoringPlatform.Domain.Common;
using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Domain.Entities;

public class TutoringSession : BaseEntity
{
    public Guid TutorId { get; set; }
    public Guid StudentId { get; set; }
    public Guid SubjectId { get; set; }
    public DateTime ScheduledStartUtc { get; set; }
    public DateTime ScheduledEndUtc { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Pending;
    public string? Notes { get; set; }

    public User Tutor { get; set; } = null!;
    public User Student { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}
