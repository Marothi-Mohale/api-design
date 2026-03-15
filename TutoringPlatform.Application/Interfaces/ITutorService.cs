using TutoringPlatform.Application.Common;
using TutoringPlatform.Application.Tutors;

namespace TutoringPlatform.Application.Interfaces;

public interface ITutorService
{
    Task<PagedResult<TutorListItemDto>> GetTutorsAsync(TutorQueryParameters query, CancellationToken cancellationToken = default);
    Task<TutorDetailDto> GetTutorByIdAsync(Guid tutorUserId, CancellationToken cancellationToken = default);
    Task<TutorDetailDto> UpsertProfileAsync(Guid tutorUserId, UpsertTutorProfileRequestDto request, CancellationToken cancellationToken = default);
}
