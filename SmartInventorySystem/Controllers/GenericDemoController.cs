using Microsoft.AspNetCore.Mvc;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Services;

namespace SmartInventorySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenericDemoController : ControllerBase
{
    private readonly ICategoryService _service;

    public GenericDemoController(
        ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data =
            await _service.GetCategories();

        return Ok(data);
    }
}
