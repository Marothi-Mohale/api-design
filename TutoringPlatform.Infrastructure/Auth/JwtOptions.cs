namespace TutoringPlatform.Infrastructure.Auth;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "TutoringPlatform";
    public string Audience { get; init; } = "TutoringPlatform.Client";
    public string Key { get; init; } = "ChangeThisKeyBeforeRunningInProduction123!";
    public int ExpirationMinutes { get; init; } = 60;
}
