using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.DTOs.Teams;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

/// <summary>Teams (graduation project teams).</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class TeamsController(ITeamService teamService) : ControllerBase
{
    /// <summary>List all teams with their members.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TeamResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() =>
        Ok(await teamService.GetAllAsync());

    /// <summary>Get one team by id.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TeamResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try { return Ok(await teamService.GetByIdAsync(id)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Create a team (starts with 0 members).</summary>
    /// <remarks>
    /// ```json
    /// {
    ///   "name": "Team Alpha",
    ///   "projectName": "SHA Senior Graduation",
    ///   "description": "…",
    ///   "logoUrl": "https://...",
    ///   "coverUrl": "https://...",
    ///   "teamNumber": 8
    /// }
    /// ```
    /// Only `name` is required (2–100 chars). `teamNumber` must be unique.
    /// Students join with `POST /api/students/me/team/{teamId}`.
    /// </remarks>
    /// <response code="409">`teamNumber` already used.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TeamResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateTeamDto dto)
    {
        try
        {
            var result = await teamService.CreateAsync(dto);
            return Created(string.Empty, result);
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    /// <summary>Update a team. All fields optional.</summary>
    /// <param name="id">Team GUID.</param>
    /// <param name="dto">Fields to change.</param>
    /// <response code="409">New `teamNumber` already used.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TeamResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeamDto dto)
    {
        try { return Ok(await teamService.UpdateAsync(id, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    /// <summary>Delete a team (members lose their team).</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try { await teamService.DeleteAsync(id); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
