namespace TutoringPlatform.Application.Auth;

public class AuthResponseDto
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
    public UserSummaryDto User { get; init; } = new();
}
