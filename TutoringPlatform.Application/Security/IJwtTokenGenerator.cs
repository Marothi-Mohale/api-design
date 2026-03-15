using TutoringPlatform.Domain.Entities;

namespace TutoringPlatform.Application.Security;

public interface IJwtTokenGenerator
{
    (string token, DateTime expiresAtUtc) GenerateToken(User user);
}
