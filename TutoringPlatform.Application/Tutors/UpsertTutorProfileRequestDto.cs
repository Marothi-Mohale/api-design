using System.ComponentModel.DataAnnotations;

namespace TutoringPlatform.Application.Tutors;

public class UpsertTutorProfileRequestDto
{
    [Required]
    [MaxLength(140)]
    public string Headline { get; init; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Bio { get; init; } = string.Empty;

    [Range(1, 10000)]
    public decimal HourlyRate { get; init; }

    [Range(0, 60)]
    public int YearsOfExperience { get; init; }

    [MinLength(1)]
    public IReadOnlyCollection<Guid> SubjectIds { get; init; } = [];
}
