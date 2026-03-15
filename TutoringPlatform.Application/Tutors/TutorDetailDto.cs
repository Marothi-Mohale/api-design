namespace TutoringPlatform.Application.Tutors;

public class TutorDetailDto
{
    public Guid UserId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Headline { get; init; } = string.Empty;
    public string Bio { get; init; } = string.Empty;
    public decimal HourlyRate { get; init; }
    public int YearsOfExperience { get; init; }
    public IReadOnlyCollection<TutorSubjectDto> Subjects { get; init; } = [];
}
