using Microsoft.AspNetCore.Mvc;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;

namespace SmartInventorySystem.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _service.GetCategories();
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Category category)
    {
        var result = await _service.AddCategory(category);
        return Ok(result);
    }
}
