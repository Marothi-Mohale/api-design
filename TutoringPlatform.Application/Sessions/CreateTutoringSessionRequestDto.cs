using System.ComponentModel.DataAnnotations;

namespace TutoringPlatform.Application.Sessions;

public class CreateTutoringSessionRequestDto
{
    [Required]
    public Guid TutorId { get; init; }

    [Required]
    public Guid SubjectId { get; init; }

    [Required]
    public DateTime ScheduledStartUtc { get; init; }

    [Required]
    public DateTime ScheduledEndUtc { get; init; }

    [MaxLength(1000)]
    public string? Notes { get; init; }
}
