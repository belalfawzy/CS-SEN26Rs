using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.DTOs.Messages;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

/// <summary>Messages between students (normal or anonymous).</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class MessagesController(IMessageService messageService) : ControllerBase
{
    private Guid CurrentStudentId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Send a message.</summary>
    /// <remarks>
    /// ```json
    /// {
    ///   "content": "Hi!",
    ///   "receiverId": "GUID-of-receiver",
    ///   "isAnonymous": false
    /// }
    /// ```
    /// `content`: 1–2000 chars. You cannot message yourself.
    /// **Anonymous messages cannot be edited or deleted later.**
    /// </remarks>
    /// <response code="400">Message to yourself or invalid content.</response>
    /// <response code="404">Receiver not found.</response>
    [HttpPost]
    [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Send([FromBody] SendMessageDto dto)
    {
        try
        {
            var result = await messageService.SendAsync(dto, CurrentStudentId);
            return Created(string.Empty, result);
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Inbox (messages sent to me, newest first).</summary>
    /// <remarks>For anonymous messages, sender fields are `null`.</remarks>
    [HttpGet("received")]
    [ProducesResponseType(typeof(List<MessageResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReceived() =>
        Ok(await messageService.GetReceivedAsync(CurrentStudentId));

    /// <summary>Outbox (messages I sent, excludes anonymous).</summary>
    [HttpGet("sent")]
    [ProducesResponseType(typeof(List<MessageResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSent() =>
        Ok(await messageService.GetSentAsync(CurrentStudentId));

    /// <summary>Edit my message (not anonymous).</summary>
    /// <remarks>
    /// ```json
    /// { "content": "New text" }
    /// ```
    /// </remarks>
    /// <param name="id">Message GUID.</param>
    /// <param name="dto">New content.</param>
    /// <response code="400">Message is anonymous.</response>
    /// <response code="403">Not your message.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMessageDto dto)
    {
        try { return Ok(await messageService.UpdateAsync(id, dto, CurrentStudentId)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Delete my message (not anonymous).</summary>
    /// <response code="400">Message is anonymous.</response>
    /// <response code="403">Not your message.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try { await messageService.DeleteAsync(id, CurrentStudentId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
