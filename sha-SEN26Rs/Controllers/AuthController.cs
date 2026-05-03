using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.DTOs.Auth;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

/// <summary>Sign up and log in.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Create a new account with email + password.</summary>
    /// <remarks>
    /// ```json
    /// { "email": "user@example.com", "password": "min6chars" }
    /// ```
    /// Save the returned `token`. Next step: `POST /api/students/me/onboarding`.
    /// </remarks>
    /// <response code="201">Account created. Returns `token` + `student`.</response>
    /// <response code="409">Email already used.</response>
    /// <response code="400">Invalid email or password too short.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var result = await authService.RegisterAsync(dto);
            return Created(string.Empty, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>Log in and get a JWT token.</summary>
    /// <remarks>
    /// ```json
    /// { "email": "user@example.com", "password": "mypassword" }
    /// ```
    /// Save `token` and send it as `Authorization: Bearer &lt;token&gt;`.
    /// Check `student.isOnboarded` → if `false`, go to onboarding page.
    /// </remarks>
    /// <response code="200">Returns `token` + `student`.</response>
    /// <response code="401">Wrong email or password.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
