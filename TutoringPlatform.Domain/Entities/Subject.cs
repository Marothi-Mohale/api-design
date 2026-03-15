using TutoringPlatform.Domain.Common;

namespace TutoringPlatform.Domain.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<TutorSubject> TutorSubjects { get; set; } = [];
    public ICollection<TutoringSession> TutoringSessions { get; set; } = [];
}
