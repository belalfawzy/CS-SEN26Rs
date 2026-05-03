using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.DTOs.Students;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

[ApiController]
[Route("api/students")]
[Authorize]
public class StudentsController(IStudentService studentService, IUserImageService imageService) : ControllerBase
{
    private Guid CurrentStudentId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await studentService.GetAllAsync());

    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        try { return Ok(await studentService.GetByUsernameAsync(username)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("{username}/images")]
    public async Task<IActionResult> GetStudentImages(string username)
    {
        try
        {
            var student = await studentService.GetByUsernameAsync(username);
            return Ok(await imageService.GetPublicByStudentIdAsync(student.Id));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        try { return Ok(await studentService.GetByIdAsync(CurrentStudentId)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPut("me")]
    public async Task<IActionResult> Update([FromBody] UpdateStudentDto dto)
    {
        try { return Ok(await studentService.UpdateAsync(CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("me/onboarding")]
    public async Task<IActionResult> Onboard([FromBody] OnboardingDto dto)
    {
        try { return Ok(await studentService.OnboardAsync(CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPost("me/team/{teamId}")]
    public async Task<IActionResult> JoinTeam(Guid teamId)
    {
        try { await studentService.JoinTeamAsync(CurrentStudentId, teamId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpDelete("me/team")]
    public async Task<IActionResult> LeaveTeam()
    {
        try { await studentService.LeaveTeamAsync(CurrentStudentId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPost("me/specialties/{specialtyId}")]
    public async Task<IActionResult> AddSpecialty(long specialtyId)
    {
        try { await studentService.AddSpecialtyAsync(CurrentStudentId, specialtyId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpDelete("me/specialties/{specialtyId}")]
    public async Task<IActionResult> RemoveSpecialty(long specialtyId)
    {
        try { await studentService.RemoveSpecialtyAsync(CurrentStudentId, specialtyId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPost("me/social-links")]
    public async Task<IActionResult> AddSocialLink([FromBody] SocialLinkInputDto dto)
    {
        try { return Ok(await studentService.AddSocialLinkAsync(CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpDelete("me/social-links/{linkId}")]
    public async Task<IActionResult> RemoveSocialLink(long linkId)
    {
        try { await studentService.RemoveSocialLinkAsync(CurrentStudentId, linkId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
