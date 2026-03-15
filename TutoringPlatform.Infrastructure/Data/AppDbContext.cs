using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Domain.Common;
using TutoringPlatform.Domain.Entities;
using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<TutorProfile> TutorProfiles => Set<TutorProfile>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<TutorSubject> TutorSubjects => Set<TutorSubject>();
    public DbSet<TutoringSession> TutoringSessions => Set<TutoringSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.Role).HasConversion<string>().HasMaxLength(20).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<StudentProfile>(builder =>
        {
            builder.ToTable("student_profiles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.GradeLevel).HasMaxLength(50);
            builder.HasOne(x => x.User)
                .WithOne(x => x.StudentProfile)
                .HasForeignKey<StudentProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TutorProfile>(builder =>
        {
            builder.ToTable("tutor_profiles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Headline).HasMaxLength(140).IsRequired();
            builder.Property(x => x.Bio).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.HourlyRate).HasPrecision(10, 2);
            builder.HasOne(x => x.User)
                .WithOne(x => x.TutorProfile)
                .HasForeignKey<TutorProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Subject>(builder =>
        {
            builder.ToTable("subjects");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<TutorSubject>(builder =>
        {
            builder.ToTable("tutor_subjects");
            builder.HasKey(x => new { x.TutorProfileId, x.SubjectId });
            builder.HasOne(x => x.TutorProfile)
                .WithMany(x => x.TutorSubjects)
                .HasForeignKey(x => x.TutorProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.TutorSubjects)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TutoringSession>(builder =>
        {
            builder.ToTable("tutoring_sessions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(SessionStatus.Pending);
            builder.Property(x => x.Notes).HasMaxLength(1000);
            builder.HasOne(x => x.Tutor)
                .WithMany(x => x.TutoringSessionsAsTutor)
                .HasForeignKey(x => x.TutorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Student)
                .WithMany(x => x.TutoringSessionsAsStudent)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.TutoringSessions)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not BaseEntity entity)
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAtUtc = DateTime.UtcNow;
            }

            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entity.UpdatedAtUtc = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
