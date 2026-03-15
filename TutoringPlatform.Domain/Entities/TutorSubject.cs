namespace TutoringPlatform.Domain.Entities;

public class TutorSubject
{
    public Guid TutorProfileId { get; set; }
    public Guid SubjectId { get; set; }

    public TutorProfile TutorProfile { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}
