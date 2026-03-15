using TutoringPlatform.Domain.Common;

namespace TutoringPlatform.Domain.Entities;

public class StudentProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public string? GradeLevel { get; set; }

    public User User { get; set; } = null!;
}
