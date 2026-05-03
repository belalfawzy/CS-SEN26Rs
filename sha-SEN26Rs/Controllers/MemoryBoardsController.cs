using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.DTOs.MemoryBoards;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

[ApiController]
[Route("api/memory-boards")]
[Authorize]
public class MemoryBoardsController(IMemoryBoardService boardService) : ControllerBase
{
    private Guid CurrentStudentId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBoard() =>
        Ok(await boardService.GetOrCreateAsync(CurrentStudentId));

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetByStudentId(Guid studentId)
    {
        try { return Ok(await boardService.GetByStudentIdAsync(studentId)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPut("my")]
    public async Task<IActionResult> UpdateMyBoard([FromBody] UpdateMemoryBoardDto dto)
    {
        try { return Ok(await boardService.UpdateAsync(CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPost("{boardId}/items")]
    public async Task<IActionResult> AddItem(Guid boardId, [FromBody] CreateBoardItemDto dto)
    {
        try
        {
            var result = await boardService.AddItemAsync(boardId, CurrentStudentId, dto);
            return Created(string.Empty, result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPut("items/{itemId}")]
    public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateBoardItemDto dto)
    {
        try { return Ok(await boardService.UpdateItemAsync(itemId, CurrentStudentId, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    [HttpDelete("items/{itemId}")]
    public async Task<IActionResult> DeleteItem(Guid itemId)
    {
        try { await boardService.DeleteItemAsync(itemId, CurrentStudentId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }
}
