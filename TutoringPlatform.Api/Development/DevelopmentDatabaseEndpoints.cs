using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Infrastructure.Data;

namespace TutoringPlatform.Api.Development;

public static class DevelopmentDatabaseEndpoints
{
    public static IEndpointRouteBuilder MapDevelopmentDatabaseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/dev");

        group.MapPost("/migrate", async Task<IResult> (
            AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
                return TypedResults.Ok(new { message = "Database migrations applied successfully." });
            }
            catch (Exception exception)
            {
                return TypedResults.Problem($"Failed to apply migrations: {exception.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        group.MapPost("/seed", async Task<IResult> (
            DevelopmentDataSeeder seeder,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await seeder.SeedAsync(cancellationToken);
                return TypedResults.Ok(new { message = "Development seed completed successfully." });
            }
            catch (Exception exception)
            {
                return TypedResults.Problem($"Failed to seed development data: {exception.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        return endpoints;
    }
}
