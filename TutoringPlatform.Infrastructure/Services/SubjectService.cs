using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Application.Common.Exceptions;
using TutoringPlatform.Application.Interfaces;
using TutoringPlatform.Application.Mappings;
using TutoringPlatform.Application.Subjects;
using TutoringPlatform.Domain.Entities;
using TutoringPlatform.Infrastructure.Data;

namespace TutoringPlatform.Infrastructure.Services;

public class SubjectService(AppDbContext dbContext) : ISubjectService
{
    public async Task<IReadOnlyCollection<SubjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var subjects = await dbContext.Subjects
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return subjects.Select(x => x.ToDto()).ToArray();
    }

    public async Task<SubjectDto> CreateAsync(CreateSubjectRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedName = request.Name.Trim();
        var exists = await dbContext.Subjects.AnyAsync(x => x.Name == normalizedName, cancellationToken);
        if (exists)
        {
            throw new ConflictException("A subject with this name already exists.");
        }

        var subject = new Subject
        {
            Name = normalizedName,
            Description = request.Description.Trim()
        };

        dbContext.Subjects.Add(subject);
        await dbContext.SaveChangesAsync(cancellationToken);

        return subject.ToDto();
    }
}
