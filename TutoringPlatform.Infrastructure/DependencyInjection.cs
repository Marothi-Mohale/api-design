using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutoringPlatform.Application.Interfaces;
using TutoringPlatform.Application.Security;
using TutoringPlatform.Infrastructure.Auth;
using TutoringPlatform.Infrastructure.Data;
using TutoringPlatform.Infrastructure.Services;

namespace TutoringPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<ITutorService, TutorService>();
        services.AddScoped<ISessionService, SessionService>();

        return services;
    }
}
