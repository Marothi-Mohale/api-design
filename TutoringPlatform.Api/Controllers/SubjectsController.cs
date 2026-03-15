using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Application.Interfaces;
using TutoringPlatform.Application.Subjects;

namespace TutoringPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SubjectsController(ISubjectService subjectService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyCollection<SubjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<SubjectDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await subjectService.GetAllAsync(cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SubjectDto>> Create([FromBody] CreateSubjectRequestDto request, CancellationToken cancellationToken)
    {
        var response = await subjectService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = response.Id }, response);
    }
}
