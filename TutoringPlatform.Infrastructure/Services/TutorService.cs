using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Application.Common;
using TutoringPlatform.Application.Common.Exceptions;
using TutoringPlatform.Application.Interfaces;
using TutoringPlatform.Application.Mappings;
using TutoringPlatform.Application.Tutors;
using TutoringPlatform.Domain.Entities;
using TutoringPlatform.Domain.Enums;
using TutoringPlatform.Infrastructure.Data;

namespace TutoringPlatform.Infrastructure.Services;

public class TutorService(AppDbContext dbContext) : ITutorService
{
    public async Task<PagedResult<TutorListItemDto>> GetTutorsAsync(TutorQueryParameters query, CancellationToken cancellationToken = default)
    {
        var tutorsQuery = dbContext.Users
            .AsNoTracking()
            .Include(x => x.TutorProfile)!
                .ThenInclude(x => x.TutorSubjects)
                .ThenInclude(x => x.Subject)
            .Where(x => x.Role == UserRole.Tutor && x.TutorProfile != null);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim().ToLower();
            tutorsQuery = tutorsQuery.Where(x =>
                (x.FirstName + " " + x.LastName).ToLower().Contains(searchTerm) ||
                x.TutorProfile!.Headline.ToLower().Contains(searchTerm) ||
                x.TutorProfile.Bio.ToLower().Contains(searchTerm));
        }

        if (query.SubjectId.HasValue)
        {
            tutorsQuery = tutorsQuery.Where(x => x.TutorProfile!.TutorSubjects.Any(ts => ts.SubjectId == query.SubjectId.Value));
        }

        if (query.MinHourlyRate.HasValue)
        {
            tutorsQuery = tutorsQuery.Where(x => x.TutorProfile!.HourlyRate >= query.MinHourlyRate.Value);
        }

        if (query.MaxHourlyRate.HasValue)
        {
            tutorsQuery = tutorsQuery.Where(x => x.TutorProfile!.HourlyRate <= query.MaxHourlyRate.Value);
        }

        tutorsQuery = (query.SortBy.ToLowerInvariant(), query.SortDirection.ToLowerInvariant()) switch
        {
            ("name", "desc") => tutorsQuery.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName),
            ("name", _) => tutorsQuery.OrderBy(x => x.LastName).ThenBy(x => x.FirstName),
            ("experience", "desc") => tutorsQuery.OrderByDescending(x => x.TutorProfile!.YearsOfExperience),
            ("experience", _) => tutorsQuery.OrderBy(x => x.TutorProfile!.YearsOfExperience),
            (_, "desc") => tutorsQuery.OrderByDescending(x => x.TutorProfile!.HourlyRate),
            _ => tutorsQuery.OrderBy(x => x.TutorProfile!.HourlyRate)
        };

        var totalCount = await tutorsQuery.CountAsync(cancellationToken);
        var tutors = await tutorsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TutorListItemDto>
        {
            Items = tutors.Select(x => x.ToListItemDto()).ToArray(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<TutorDetailDto> GetTutorByIdAsync(Guid tutorUserId, CancellationToken cancellationToken = default)
    {
        var tutor = await GetTutorQueryable()
            .FirstOrDefaultAsync(x => x.Id == tutorUserId, cancellationToken)
            ?? throw new NotFoundException("Tutor was not found.");

        return tutor.ToDetailDto();
    }

    public async Task<TutorDetailDto> UpsertProfileAsync(Guid tutorUserId, UpsertTutorProfileRequestDto request, CancellationToken cancellationToken = default)
    {
        var tutor = await dbContext.Users
            .Include(x => x.TutorProfile)!
                .ThenInclude(x => x.TutorSubjects)
            .FirstOrDefaultAsync(x => x.Id == tutorUserId, cancellationToken)
            ?? throw new NotFoundException("Tutor account was not found.");

        if (tutor.Role != UserRole.Tutor)
        {
            throw new ForbiddenException("Only tutors can manage tutor profiles.");
        }

        var subjectIds = request.SubjectIds.Distinct().ToArray();
        var subjects = await dbContext.Subjects
            .Where(x => subjectIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (subjects.Count != subjectIds.Length)
        {
            throw new ValidationException("One or more selected subjects do not exist.");
        }

        var profile = tutor.TutorProfile ?? new TutorProfile { UserId = tutorUserId };
        profile.Headline = request.Headline.Trim();
        profile.Bio = request.Bio.Trim();
        profile.HourlyRate = request.HourlyRate;
        profile.YearsOfExperience = request.YearsOfExperience;

        if (tutor.TutorProfile is null)
        {
            tutor.TutorProfile = profile;
        }

        profile.TutorSubjects.Clear();
        foreach (var subjectId in subjectIds)
        {
            profile.TutorSubjects.Add(new TutorSubject
            {
                TutorProfileId = profile.Id,
                SubjectId = subjectId
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var savedTutor = await GetTutorQueryable().FirstAsync(x => x.Id == tutorUserId, cancellationToken);
        return savedTutor.ToDetailDto();
    }

    private IQueryable<User> GetTutorQueryable()
    {
        return dbContext.Users
            .AsNoTracking()
            .Include(x => x.TutorProfile)!
                .ThenInclude(x => x.TutorSubjects)
                .ThenInclude(x => x.Subject)
            .Where(x => x.Role == UserRole.Tutor && x.TutorProfile != null);
    }
}
