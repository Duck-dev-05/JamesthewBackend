using JamesThew.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RecipesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRecipes()
    {
        return Ok(await _context.Recipes.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] Recipe model)
    {
        _context.Recipes.Add(model);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Recipe created successfully" });
    }
}
