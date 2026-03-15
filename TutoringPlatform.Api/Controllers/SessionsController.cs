using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Api.Auth;
using TutoringPlatform.Application.Common;
using TutoringPlatform.Application.Interfaces;
using TutoringPlatform.Application.Sessions;

namespace TutoringPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class SessionsController(ISessionService sessionService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Student")]
    [ProducesResponseType(typeof(TutoringSessionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TutoringSessionDto>> Create([FromBody] CreateTutoringSessionRequestDto request, CancellationToken cancellationToken)
    {
        var studentId = User.GetUserId();
        var response = await sessionService.CreateAsync(studentId, request, cancellationToken);
        return CreatedAtAction(nameof(GetMySessions), new { pageNumber = 1, pageSize = 10 }, response);
    }

    [HttpGet("my")]
    [ProducesResponseType(typeof(PagedResult<TutoringSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResult<TutoringSessionDto>>> GetMySessions([FromQuery] SessionQueryParameters query, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var role = User.GetUserRole();
        return Ok(await sessionService.GetMySessionsAsync(userId, role, query, cancellationToken));
    }

    [HttpPatch("{sessionId:guid}/status")]
    [Authorize(Roles = "Tutor,Admin")]
    [ProducesResponseType(typeof(TutoringSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutoringSessionDto>> UpdateStatus(Guid sessionId, [FromBody] UpdateSessionStatusRequestDto request, CancellationToken cancellationToken)
    {
        var actorId = User.GetUserId();
        var role = User.GetUserRole();
        return Ok(await sessionService.UpdateStatusAsync(actorId, role, sessionId, request, cancellationToken));
    }
}
