using System.ComponentModel.DataAnnotations;

namespace TutoringPlatform.Application.Auth;

public class RegisterUserRequestDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; init; } = string.Empty;

    [Required]
    [RegularExpression("Student|Tutor", ErrorMessage = "Role must be either Student or Tutor.")]
    public string Role { get; init; } = string.Empty;

    [MaxLength(50)]
    public string? GradeLevel { get; init; }
}
