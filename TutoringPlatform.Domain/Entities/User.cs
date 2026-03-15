using TutoringPlatform.Domain.Common;
using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public TutorProfile? TutorProfile { get; set; }
    public StudentProfile? StudentProfile { get; set; }
    public ICollection<TutoringSession> TutoringSessionsAsTutor { get; set; } = [];
    public ICollection<TutoringSession> TutoringSessionsAsStudent { get; set; } = [];
}
