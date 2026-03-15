using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Application.Common;
using TutoringPlatform.Application.Common.Exceptions;
using TutoringPlatform.Application.Interfaces;
using TutoringPlatform.Application.Mappings;
using TutoringPlatform.Application.Sessions;
using TutoringPlatform.Domain.Entities;
using TutoringPlatform.Domain.Enums;
using TutoringPlatform.Infrastructure.Data;

namespace TutoringPlatform.Infrastructure.Services;

public class SessionService(AppDbContext dbContext) : ISessionService
{
    public async Task<TutoringSessionDto> CreateAsync(Guid studentId, CreateTutoringSessionRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.ScheduledEndUtc <= request.ScheduledStartUtc)
        {
            throw new ValidationException("Scheduled end time must be after the start time.");
        }

        if (request.ScheduledStartUtc <= DateTime.UtcNow)
        {
            throw new ValidationException("Sessions must be scheduled in the future.");
        }

        var student = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == studentId, cancellationToken)
            ?? throw new NotFoundException("Student account was not found.");

        if (student.Role != UserRole.Student)
        {
            throw new ForbiddenException("Only students can request tutoring sessions.");
        }

        var tutor = await dbContext.Users
            .Include(x => x.TutorProfile)!
                .ThenInclude(x => x.TutorSubjects)
            .FirstOrDefaultAsync(x => x.Id == request.TutorId, cancellationToken)
            ?? throw new NotFoundException("Tutor account was not found.");

        if (tutor.Role != UserRole.Tutor || tutor.TutorProfile is null)
        {
            throw new ValidationException("The selected tutor does not have an active tutor profile.");
        }

        var teachesSubject = tutor.TutorProfile.TutorSubjects.Any(x => x.SubjectId == request.SubjectId);
        if (!teachesSubject)
        {
            throw new ValidationException("The selected tutor does not teach this subject.");
        }

        var subject = await dbContext.Subjects.FirstOrDefaultAsync(x => x.Id == request.SubjectId, cancellationToken)
            ?? throw new NotFoundException("Subject was not found.");

        var session = new TutoringSession
        {
            TutorId = tutor.Id,
            StudentId = studentId,
            SubjectId = subject.Id,
            ScheduledStartUtc = request.ScheduledStartUtc,
            ScheduledEndUtc = request.ScheduledEndUtc,
            Notes = request.Notes?.Trim()
        };

        dbContext.TutoringSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        var savedSession = await GetSessionQueryable()
            .FirstAsync(x => x.Id == session.Id, cancellationToken);

        return savedSession.ToDto();
    }

    public async Task<PagedResult<TutoringSessionDto>> GetMySessionsAsync(Guid userId, UserRole role, SessionQueryParameters query, CancellationToken cancellationToken = default)
    {
        if (role is not UserRole.Student and not UserRole.Tutor and not UserRole.Admin)
        {
            throw new ForbiddenException("This account cannot access tutoring sessions.");
        }

        var sessionsQuery = GetSessionQueryable();

        sessionsQuery = role switch
        {
            UserRole.Tutor => sessionsQuery.Where(x => x.TutorId == userId),
            UserRole.Student => sessionsQuery.Where(x => x.StudentId == userId),
            _ => sessionsQuery
        };

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            if (!Enum.TryParse<SessionStatus>(query.Status, true, out var status))
            {
                throw new ValidationException("Invalid session status filter.");
            }

            sessionsQuery = sessionsQuery.Where(x => x.Status == status);
        }

        if (query.UpcomingOnly)
        {
            sessionsQuery = sessionsQuery.Where(x => x.ScheduledStartUtc >= DateTime.UtcNow);
        }

        sessionsQuery = query.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? sessionsQuery.OrderByDescending(x => x.ScheduledStartUtc)
            : sessionsQuery.OrderBy(x => x.ScheduledStartUtc);

        var totalCount = await sessionsQuery.CountAsync(cancellationToken);
        var sessions = await sessionsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TutoringSessionDto>
        {
            Items = sessions.Select(x => x.ToDto()).ToArray(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<TutoringSessionDto> UpdateStatusAsync(Guid actorId, UserRole actorRole, Guid sessionId, UpdateSessionStatusRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<SessionStatus>(request.Status, true, out var status))
        {
            throw new ValidationException("Invalid session status.");
        }

        var session = await dbContext.TutoringSessions
            .Include(x => x.Tutor)
            .Include(x => x.Student)
            .Include(x => x.Subject)
            .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken)
            ?? throw new NotFoundException("Tutoring session was not found.");

        var canManageSession = actorRole == UserRole.Admin || (actorRole == UserRole.Tutor && session.TutorId == actorId);
        if (!canManageSession)
        {
            throw new ForbiddenException("Only the assigned tutor or an admin can update the session status.");
        }

        session.Status = status;
        await dbContext.SaveChangesAsync(cancellationToken);

        return session.ToDto();
    }

    private IQueryable<TutoringSession> GetSessionQueryable()
    {
        return dbContext.TutoringSessions
            .AsNoTracking()
            .Include(x => x.Tutor)
            .Include(x => x.Student)
            .Include(x => x.Subject);
    }
}
