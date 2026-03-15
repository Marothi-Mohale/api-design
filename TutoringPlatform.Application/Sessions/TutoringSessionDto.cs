using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Application.Sessions;

public class TutoringSessionDto
{
    public Guid Id { get; init; }
    public Guid TutorId { get; init; }
    public string TutorName { get; init; } = string.Empty;
    public Guid StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public Guid SubjectId { get; init; }
    public string SubjectName { get; init; } = string.Empty;
    public DateTime ScheduledStartUtc { get; init; }
    public DateTime ScheduledEndUtc { get; init; }
    public SessionStatus Status { get; init; }
    public string? Notes { get; init; }
}
