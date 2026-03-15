using System.ComponentModel.DataAnnotations;

namespace TutoringPlatform.Application.Subjects;

public class CreateSubjectRequestDto
{
    [Required]
    [MaxLength(120)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; init; } = string.Empty;
}
