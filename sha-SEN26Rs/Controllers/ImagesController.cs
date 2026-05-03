using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

/// <summary>Upload and manage photos.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ImagesController(IUserImageService imageService) : ControllerBase
{
    private Guid CurrentStudentId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Upload one image (multipart/form-data).</summary>
    /// <remarks>
    /// Send as `FormData`, not JSON. Do **not** set `Content-Type` yourself.
    /// ```js
    /// const form = new FormData();
    /// form.append("file", fileInput.files[0]);
    /// form.append("caption", "My first day");  // optional
    /// form.append("isPublic", "true");          // optional, default false
    /// ```
    /// Allowed: JPEG, PNG, WebP, GIF. Max **5 MB**.
    /// For avatar/cover: upload here, then put the returned `url` into `PUT /api/students/me`.
    /// </remarks>
    /// <response code="400">Empty file, too big, or wrong type.</response>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UserImageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] string? caption,
        [FromForm] bool isPublic = false)
    {
        try
        {
            var result = await imageService.UploadAsync(CurrentStudentId, file, caption, isPublic);
            return Created(string.Empty, result);
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>All my photos (public + private).</summary>
    [HttpGet("my")]
    [ProducesResponseType(typeof(List<UserImageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyImages() =>
        Ok(await imageService.GetMyImagesAsync(CurrentStudentId));

    /// <summary>All public photos from everyone (community feed, newest first).</summary>
    [HttpGet("community")]
    [ProducesResponseType(typeof(List<CommunityImageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommunity() =>
        Ok(await imageService.GetCommunityImagesAsync());

    /// <summary>Delete one of my photos.</summary>
    /// <remarks>You can only delete your own photos.</remarks>
    /// <response code="403">Not your photo.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try { await imageService.DeleteAsync(id, CurrentStudentId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }
}
