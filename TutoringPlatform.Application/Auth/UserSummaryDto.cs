using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Application.Auth;

public class UserSummaryDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public UserRole Role { get; init; }
}
