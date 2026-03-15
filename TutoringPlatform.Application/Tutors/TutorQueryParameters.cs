using TutoringPlatform.Application.Common;

namespace TutoringPlatform.Application.Tutors;

public class TutorQueryParameters : PagedRequest
{
    public string? SearchTerm { get; init; }
    public Guid? SubjectId { get; init; }
    public decimal? MinHourlyRate { get; init; }
    public decimal? MaxHourlyRate { get; init; }
    public string SortBy { get; init; } = "hourlyRate";
    public string SortDirection { get; init; } = "asc";
}
