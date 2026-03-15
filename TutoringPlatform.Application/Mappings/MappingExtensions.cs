using TutoringPlatform.Application.Auth;
using TutoringPlatform.Application.Sessions;
using TutoringPlatform.Application.Subjects;
using TutoringPlatform.Application.Tutors;
using TutoringPlatform.Domain.Entities;

namespace TutoringPlatform.Application.Mappings;

public static class MappingExtensions
{
    public static UserSummaryDto ToSummaryDto(this User user) =>
        new()
        {
            Id = user.Id,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Role = user.Role
        };

    public static SubjectDto ToDto(this Subject subject) =>
        new()
        {
            Id = subject.Id,
            Name = subject.Name,
            Description = subject.Description
        };

    public static TutorListItemDto ToListItemDto(this User tutor) =>
        new()
        {
            UserId = tutor.Id,
            FullName = $"{tutor.FirstName} {tutor.LastName}".Trim(),
            Headline = tutor.TutorProfile?.Headline ?? string.Empty,
            HourlyRate = tutor.TutorProfile?.HourlyRate ?? 0,
            YearsOfExperience = tutor.TutorProfile?.YearsOfExperience ?? 0,
            Subjects = tutor.TutorProfile?.TutorSubjects
                .Select(x => x.Subject.Name)
                .OrderBy(x => x)
                .ToArray() ?? []
        };

    public static TutorDetailDto ToDetailDto(this User tutor) =>
        new()
        {
            UserId = tutor.Id,
            FullName = $"{tutor.FirstName} {tutor.LastName}".Trim(),
            Email = tutor.Email,
            Headline = tutor.TutorProfile?.Headline ?? string.Empty,
            Bio = tutor.TutorProfile?.Bio ?? string.Empty,
            HourlyRate = tutor.TutorProfile?.HourlyRate ?? 0,
            YearsOfExperience = tutor.TutorProfile?.YearsOfExperience ?? 0,
            Subjects = tutor.TutorProfile?.TutorSubjects
                .Select(x => new TutorSubjectDto
                {
                    SubjectId = x.SubjectId,
                    Name = x.Subject.Name
                })
                .OrderBy(x => x.Name)
                .ToArray() ?? []
        };

    public static TutoringSessionDto ToDto(this TutoringSession session) =>
        new()
        {
            Id = session.Id,
            TutorId = session.TutorId,
            TutorName = $"{session.Tutor.FirstName} {session.Tutor.LastName}".Trim(),
            StudentId = session.StudentId,
            StudentName = $"{session.Student.FirstName} {session.Student.LastName}".Trim(),
            SubjectId = session.SubjectId,
            SubjectName = session.Subject.Name,
            ScheduledStartUtc = session.ScheduledStartUtc,
            ScheduledEndUtc = session.ScheduledEndUtc,
            Status = session.Status,
            Notes = session.Notes
        };
}
