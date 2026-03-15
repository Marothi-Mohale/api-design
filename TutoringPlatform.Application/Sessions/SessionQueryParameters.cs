using TutoringPlatform.Application.Common;

namespace TutoringPlatform.Application.Sessions;

public class SessionQueryParameters : PagedRequest
{
    public string? Status { get; init; }
    public bool UpcomingOnly { get; init; }
    public string SortDirection { get; init; } = "asc";
}
