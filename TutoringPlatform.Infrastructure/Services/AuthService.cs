using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TutoringPlatform.Application.Auth;
using TutoringPlatform.Application.Common.Exceptions;
using TutoringPlatform.Application.Interfaces;
using TutoringPlatform.Application.Mappings;
using TutoringPlatform.Application.Security;
using TutoringPlatform.Domain.Entities;
using TutoringPlatform.Domain.Enums;
using TutoringPlatform.Infrastructure.Data;

namespace TutoringPlatform.Infrastructure.Services;

public class AuthService(
    AppDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailExists = await dbContext.Users.AnyAsync(x => x.Email == normalizedEmail, cancellationToken);
        if (emailExists)
        {
            throw new ConflictException("An account with this email address already exists.");
        }

        if (!Enum.TryParse<UserRole>(request.Role, true, out var role) || role == UserRole.Admin)
        {
            throw new ValidationException("Only Student or Tutor registration is allowed.");
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = normalizedEmail,
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = role
        };

        if (role == UserRole.Student)
        {
            user.StudentProfile = new StudentProfile
            {
                GradeLevel = request.GradeLevel?.Trim()
            };
        }

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Registered new user {Email} with role {Role}", user.Email, user.Role);

        var (token, expiresAtUtc) = jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc,
            User = user.ToSummaryDto()
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken)
            ?? throw new ValidationException("Invalid email or password.");

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new ValidationException("Invalid email or password.");
        }

        logger.LogInformation("User {Email} authenticated successfully", user.Email);

        var (token, expiresAtUtc) = jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc,
            User = user.ToSummaryDto()
        };
    }
}
