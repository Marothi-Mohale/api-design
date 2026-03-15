using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Api.Auth;
using TutoringPlatform.Application.Common;
using TutoringPlatform.Application.Interfaces;
using TutoringPlatform.Application.Tutors;

namespace TutoringPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TutorsController(ITutorService tutorService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<TutorListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TutorListItemDto>>> GetTutors([FromQuery] TutorQueryParameters query, CancellationToken cancellationToken)
    {
        return Ok(await tutorService.GetTutorsAsync(query, cancellationToken));
    }

    [HttpGet("{tutorUserId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TutorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorDetailDto>> GetTutorById(Guid tutorUserId, CancellationToken cancellationToken)
    {
        return Ok(await tutorService.GetTutorByIdAsync(tutorUserId, cancellationToken));
    }

    [HttpPut("profile")]
    [Authorize(Roles = "Tutor")]
    [ProducesResponseType(typeof(TutorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TutorDetailDto>> UpsertProfile([FromBody] UpsertTutorProfileRequestDto request, CancellationToken cancellationToken)
    {
        var tutorId = User.GetUserId();
        var response = await tutorService.UpsertProfileAsync(tutorId, request, cancellationToken);
        return Ok(response);
    }
}
