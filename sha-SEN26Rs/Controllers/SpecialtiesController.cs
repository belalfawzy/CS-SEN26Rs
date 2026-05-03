using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sha_SEN26Rs.Services;

namespace sha_SEN26Rs.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SpecialtiesController(ISpecialtyService specialtyService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await specialtyService.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string name)
    {
        var result = await specialtyService.CreateAsync(name);
        return Created(string.Empty, result);
    }
}
