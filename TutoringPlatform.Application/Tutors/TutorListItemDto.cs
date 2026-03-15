namespace TutoringPlatform.Application.Tutors;

public class TutorListItemDto
{
    public Guid UserId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Headline { get; init; } = string.Empty;
    public decimal HourlyRate { get; init; }
    public int YearsOfExperience { get; init; }
    public IReadOnlyCollection<string> Subjects { get; init; } = [];
}
