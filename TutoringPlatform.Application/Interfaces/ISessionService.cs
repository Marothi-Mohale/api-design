using TutoringPlatform.Application.Common;
using TutoringPlatform.Application.Sessions;
using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Application.Interfaces;

public interface ISessionService
{
    Task<TutoringSessionDto> CreateAsync(Guid studentId, CreateTutoringSessionRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResult<TutoringSessionDto>> GetMySessionsAsync(Guid userId, UserRole role, SessionQueryParameters query, CancellationToken cancellationToken = default);
    Task<TutoringSessionDto> UpdateStatusAsync(Guid actorId, UserRole actorRole, Guid sessionId, UpdateSessionStatusRequestDto request, CancellationToken cancellationToken = default);
}
