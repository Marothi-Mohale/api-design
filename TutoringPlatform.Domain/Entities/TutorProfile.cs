using TutoringPlatform.Domain.Common;

namespace TutoringPlatform.Domain.Entities;

public class TutorProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public int YearsOfExperience { get; set; }

    public User User { get; set; } = null!;
    public ICollection<TutorSubject> TutorSubjects { get; set; } = [];
}
