using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.DTOs.MemoryBoards;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

/// <summary>Memory boards: a wall where friends drop notes, photos, and stickers at X/Y.</summary>
[ApiController]
[Route("api/memory-boards")]
[Authorize]
[Produces("application/json")]
public class MemoryBoardsController(IMemoryBoardService boardService) : ControllerBase
{
    private Guid CurrentStudentId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get my board (auto-creates if missing, so never 404).</summary>
    /// <remarks>Save the board `id`: you need it to add items.</remarks>
    [HttpGet("my")]
    [ProducesResponseType(typeof(MemoryBoardResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyBoard() =>
        Ok(await boardService.GetOrCreateAsync(CurrentStudentId));

    /// <summary>Get another student's board by their student id.</summary>
    /// <param name="studentId">Student GUID (from profile `id`).</param>
    /// <response code="404">That student has no board yet.</response>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(typeof(MemoryBoardResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByStudentId(Guid studentId)
    {
        try { return Ok(await boardService.GetByStudentIdAsync(studentId)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Update my board (title, background, size). All fields optional.</summary>
    /// <remarks>
    /// ```json
    /// {
    ///   "title": "My Senior Year",
    ///   "backgroundUrl": "https://...",
    ///   "backgroundColor": "#fff3e0",
    ///   "width": 1200,
    ///   "height": 800
    /// }
    /// ```
    /// </remarks>
    [HttpPut("my")]
    [ProducesResponseType(typeof(MemoryBoardResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMyBoard([FromBody] UpdateMemoryBoardDto dto)
    {
        try { return Ok(await boardService.UpdateAsync(CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Add an item (note, photo, sticker) to any board.</summary>
    /// <remarks>
    /// ```json
    /// {
    ///   "type": "sticky",
    ///   "content": "Good luck bro!",
    ///   "x": 120, "y": 240,
    ///   "width": 200, "height": 150,
    ///   "rotation": -5, "zIndex": 2
    /// }
    /// ```
    /// `type` is free text (`sticky`, `text`, `image`, `emoji`, …). You become the author —
    /// **only you** can edit/delete this item later.
    /// </remarks>
    /// <param name="boardId">Board GUID (yours from `GET /my`, friend's from `GET /student/{id}`).</param>
    /// <param name="dto">Item fields.</param>
    [HttpPost("{boardId}/items")]
    [ProducesResponseType(typeof(BoardItemResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem(Guid boardId, [FromBody] CreateBoardItemDto dto)
    {
        try
        {
            var result = await boardService.AddItemAsync(boardId, CurrentStudentId, dto);
            return Created(string.Empty, result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Update an item I created (move, resize, rotate, change text).</summary>
    /// <remarks>All fields optional — send only what changed.</remarks>
    /// <param name="itemId">Item GUID.</param>
    /// <param name="dto">Fields to change.</param>
    /// <response code="403">Not your item.</response>
    [HttpPut("items/{itemId}")]
    [ProducesResponseType(typeof(BoardItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateBoardItemDto dto)
    {
        try { return Ok(await boardService.UpdateItemAsync(itemId, CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    /// <summary>Delete an item I created.</summary>
    /// <response code="403">Not your item.</response>
    [HttpDelete("items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem(Guid itemId)
    {
        try { await boardService.DeleteItemAsync(itemId, CurrentStudentId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }
}
