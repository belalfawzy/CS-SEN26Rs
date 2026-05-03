using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

/// <summary>Specialties (skill tracks like "Back-End .NET", "UI/UX").</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class SpecialtiesController(ISpecialtyService specialtyService) : ControllerBase
{
    /// <summary>List all specialties. Use the returned `id` with the student endpoints.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<SpecialtyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() =>
        Ok(await specialtyService.GetAllAsync());

    /// <summary>Create a specialty (admin).</summary>
    /// <remarks>
    /// Body is a **plain string**, not a JSON object:
    /// ```json
    /// "DevOps"
    /// ```
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(SpecialtyDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] string name)
    {
        var result = await specialtyService.CreateAsync(name);
        return Created(string.Empty, result);
    }
}
