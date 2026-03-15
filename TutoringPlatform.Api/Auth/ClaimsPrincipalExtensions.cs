using System.Security.Claims;
using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Api.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var userId)
            ? userId
            : throw new UnauthorizedAccessException("The current user identifier claim is missing.");
    }

    public static UserRole GetUserRole(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.Role);
        return Enum.TryParse<UserRole>(value, true, out var role)
            ? role
            : throw new UnauthorizedAccessException("The current user role claim is missing.");
    }
}
