using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TutoringPlatform.Application.Security;
using TutoringPlatform.Domain.Entities;
using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Infrastructure.Data;

public class DevelopmentDataSeeder(
    AppDbContext dbContext,
    IPasswordHasher passwordHasher,
    ILogger<DevelopmentDataSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (await dbContext.Users.AnyAsync(cancellationToken) || await dbContext.Subjects.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Development seed skipped because the database already contains data.");
            return;
        }

        var subjects = new[]
        {
            new Subject { Name = "Mathematics", Description = "Algebra, calculus, geometry, and problem-solving support." },
            new Subject { Name = "Physics", Description = "Mechanics, electricity, waves, and exam preparation." },
            new Subject { Name = "Chemistry", Description = "Foundational chemistry concepts, lab theory, and test readiness." },
            new Subject { Name = "Computer Science", Description = "Programming fundamentals, data structures, and algorithms." }
        };

        var admin = new User
        {
            FirstName = "Platform",
            LastName = "Admin",
            Email = "admin@tutoringplatform.dev",
            PasswordHash = passwordHasher.Hash("Admin123!"),
            Role = UserRole.Admin
        };

        var tutorAda = new User
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@tutoringplatform.dev",
            PasswordHash = passwordHasher.Hash("Tutor123!"),
            Role = UserRole.Tutor
        };

        tutorAda.TutorProfile = new TutorProfile
        {
            Headline = "STEM tutor for mathematics and computing",
            Bio = "Helps students build confidence in problem solving, algebra, and introductory programming.",
            HourlyRate = 45m,
            YearsOfExperience = 6
        };

        var tutorGrace = new User
        {
            FirstName = "Grace",
            LastName = "Hopper",
            Email = "grace@tutoringplatform.dev",
            PasswordHash = passwordHasher.Hash("Tutor123!"),
            Role = UserRole.Tutor
        };

        tutorGrace.TutorProfile = new TutorProfile
        {
            Headline = "Physics and computer science exam coach",
            Bio = "Focuses on structured revision plans, fundamentals, and interview-style thinking.",
            HourlyRate = 55m,
            YearsOfExperience = 9
        };

        var student = new User
        {
            FirstName = "Jamie",
            LastName = "Student",
            Email = "student@tutoringplatform.dev",
            PasswordHash = passwordHasher.Hash("Student123!"),
            Role = UserRole.Student,
            StudentProfile = new StudentProfile
            {
                GradeLevel = "Grade 11"
            }
        };

        dbContext.Subjects.AddRange(subjects);
        dbContext.Users.AddRange(admin, tutorAda, tutorGrace, student);
        await dbContext.SaveChangesAsync(cancellationToken);

        tutorAda.TutorProfile!.TutorSubjects.Add(new TutorSubject
        {
            TutorProfileId = tutorAda.TutorProfile.Id,
            SubjectId = subjects.Single(x => x.Name == "Mathematics").Id
        });
        tutorAda.TutorProfile.TutorSubjects.Add(new TutorSubject
        {
            TutorProfileId = tutorAda.TutorProfile.Id,
            SubjectId = subjects.Single(x => x.Name == "Computer Science").Id
        });

        tutorGrace.TutorProfile!.TutorSubjects.Add(new TutorSubject
        {
            TutorProfileId = tutorGrace.TutorProfile.Id,
            SubjectId = subjects.Single(x => x.Name == "Physics").Id
        });
        tutorGrace.TutorProfile.TutorSubjects.Add(new TutorSubject
        {
            TutorProfileId = tutorGrace.TutorProfile.Id,
            SubjectId = subjects.Single(x => x.Name == "Computer Science").Id
        });

        dbContext.TutoringSessions.Add(new TutoringSession
        {
            TutorId = tutorAda.Id,
            StudentId = student.Id,
            SubjectId = subjects.Single(x => x.Name == "Mathematics").Id,
            ScheduledStartUtc = DateTime.UtcNow.AddDays(2),
            ScheduledEndUtc = DateTime.UtcNow.AddDays(2).AddHours(1),
            Status = SessionStatus.Confirmed,
            Notes = "Focus on algebra revision and quadratic equations."
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Development seed data created for Tutoring Platform API.");
    }
}
