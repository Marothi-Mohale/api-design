using TutoringPlatform.Application.Subjects;

namespace TutoringPlatform.Application.Interfaces;

public interface ISubjectService
{
    Task<IReadOnlyCollection<SubjectDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SubjectDto> CreateAsync(CreateSubjectRequestDto request, CancellationToken cancellationToken = default);
}
