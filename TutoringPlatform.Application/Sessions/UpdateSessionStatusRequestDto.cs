using System.ComponentModel.DataAnnotations;

namespace TutoringPlatform.Application.Sessions;

public class UpdateSessionStatusRequestDto
{
    [Required]
    [RegularExpression("Pending|Confirmed|Completed|Cancelled", ErrorMessage = "Status must be Pending, Confirmed, Completed, or Cancelled.")]
    public string Status { get; init; } = string.Empty;
}
