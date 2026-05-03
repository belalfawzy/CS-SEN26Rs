using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.DTOs.Students;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

/// <summary>Students and profiles.</summary>
[ApiController]
[Route("api/students")]
[Authorize]
[Produces("application/json")]
public class StudentsController(IStudentService studentService, IUserImageService imageService) : ControllerBase
{
    private Guid CurrentStudentId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>List all students (A–Z).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<StudentResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() =>
        Ok(await studentService.GetAllAsync());

    /// <summary>Search students by any profile field.</summary>
    /// <remarks>
    /// Example: `GET /api/students/search?q=belal`
    ///
    /// Matches (case-insensitive, partial): `fullName`, `username`, `nickname`, `bio`,
    /// `location`, `phone`, `website`, `graduationProjectSpecialty`, `team.name`,
    /// `team.projectName`, and specialty names.
    /// If `q` is a number, it also matches `team.teamNumber` (exact).
    ///
    /// Returns an empty array `[]` when nothing matches — show a
    /// "No users found" message in the UI.
    /// </remarks>
    /// <param name="q">Search text (required, not empty).</param>
    /// <response code="200">Matching students (can be empty).</response>
    /// <response code="400">`q` is missing or empty.</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<StudentResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        try { return Ok(await studentService.SearchAsync(q)); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Get the logged-in user's profile.</summary>
    /// <remarks>Check `isOnboarded` to decide: onboarding page vs home.</remarks>
    /// <response code="200">My profile.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(StudentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe()
    {
        try { return Ok(await studentService.GetByIdAsync(CurrentStudentId)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Get a student by their username (the @handle, not the email or id).</summary>
    /// <param name="username">The `@handle`, no `@` sign.</param>
    /// <response code="404">No student with this username.</response>
    [HttpGet("{username}")]
    [ProducesResponseType(typeof(StudentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUsername(string username)
    {
        try { return Ok(await studentService.GetByUsernameAsync(username)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Get a student's public photos.</summary>
    /// <remarks>Only photos with `isPublic: true`. For your own (public + private) use `GET /api/images/my`.</remarks>
    /// <param name="username">The student's username.</param>
    [HttpGet("{username}/images")]
    [ProducesResponseType(typeof(List<CommunityImageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentImages(string username)
    {
        try
        {
            var student = await studentService.GetByUsernameAsync(username);
            return Ok(await imageService.GetPublicByStudentIdAsync(student.Id));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Fill the profile (call once after register).</summary>
    /// <remarks>
    /// ```json
    /// {
    ///   "fullName": "Belal Fawzy",
    ///   "nickname": "Belaaal",
    ///   "bio": "CS student",
    ///   "phone": "01124259475",
    ///   "location": "Cairo, Egypt",
    ///   "website": "https://...",
    ///   "graduationProjectSpecialty": "Back-End .NET",
    ///   "privacySetting": "public",
    ///   "teamNumber": 8,
    ///   "socialLinks": [{ "platform": "GitHub", "url": "https://..." }]
    /// }
    /// ```
    /// Only `fullName` is required. `privacySetting`: `public` | `students_only` | `private`.
    /// `teamNumber` auto-creates the team if missing. After success, `isOnboarded` becomes `true`.
    /// </remarks>
    [HttpPost("me/onboarding")]
    [ProducesResponseType(typeof(StudentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Onboard([FromBody] OnboardingDto dto)
    {
        try { return Ok(await studentService.OnboardAsync(CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Update my profile. All fields optional, send only what changed.</summary>
    /// <remarks>
    /// **Rules:**
    /// - Must change at least one value, otherwise 400.
    /// - Empty string `""` becomes `null` for text fields.
    /// - `socialLinks` **replaces** the full list (send all links, not just the new one).
    /// - `teamNumber` auto-creates the team if missing.
    /// - `privacySetting`: `public` | `students_only` | `private`.
    /// </remarks>
    /// <response code="400">Nothing changed.</response>
    [HttpPut("me")]
    [ProducesResponseType(typeof(StudentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateStudentDto dto)
    {
        try { return Ok(await studentService.UpdateAsync(CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Join a team by team GUID.</summary>
    /// <remarks>Want to join by team **number** instead? Use `PUT /api/students/me` with `teamNumber`.</remarks>
    /// <param name="teamId">Team GUID (from `GET /api/teams`).</param>
    [HttpPost("me/team/{teamId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> JoinTeam(Guid teamId)
    {
        try { await studentService.JoinTeamAsync(CurrentStudentId, teamId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Leave my current team.</summary>
    [HttpDelete("me/team")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LeaveTeam()
    {
        try { await studentService.LeaveTeamAsync(CurrentStudentId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Add one social link to my profile.</summary>
    /// <remarks>
    /// ```json
    /// { "platform": "GitHub", "url": "https://github.com/username" }
    /// ```
    /// Want to replace the full list? Use `PUT /api/students/me` with `socialLinks`.
    /// </remarks>
    [HttpPost("me/social-links")]
    [ProducesResponseType(typeof(SocialLinkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSocialLink([FromBody] SocialLinkInputDto dto)
    {
        try { return Ok(await studentService.AddSocialLinkAsync(CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Delete one of my social links.</summary>
    /// <param name="linkId">The numeric `id` from `GET /api/students/me` → `socialLinks[].id`.</param>
    [HttpDelete("me/social-links/{linkId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveSocialLink(long linkId)
    {
        try { await studentService.RemoveSocialLinkAsync(CurrentStudentId, linkId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Add a specialty to my profile.</summary>
    /// <param name="specialtyId">The `id` from `GET /api/specialties`.</param>
    [HttpPost("me/specialties/{specialtyId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSpecialty(long specialtyId)
    {
        try { await studentService.AddSpecialtyAsync(CurrentStudentId, specialtyId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Remove a specialty from my profile.</summary>
    [HttpDelete("me/specialties/{specialtyId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveSpecialty(long specialtyId)
    {
        try { await studentService.RemoveSpecialtyAsync(CurrentStudentId, specialtyId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
