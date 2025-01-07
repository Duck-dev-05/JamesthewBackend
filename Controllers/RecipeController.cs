using JamesthewBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly AppDbContext _context;

    public RecipeController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Recipe
    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        try
        {
            var recipes = await _context.Recipes.ToListAsync();
            return Ok(recipes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST: api/Recipe
    [HttpPost]
    [Authorize(Roles = "Member,Admin")]
    public async Task<IActionResult> AddRecipe([FromBody] Recipe recipe)
    {
        if (recipe == null)
        {
            return BadRequest("Recipe cannot be null.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid User ID.");
            }

            recipe.UserId = userId;

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecipes), new { id = recipe.Id }, recipe);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/Recipe/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Member,Admin")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] Recipe updatedRecipe)
    {
        if (updatedRecipe == null)
        {
            return BadRequest("Updated recipe cannot be null.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            recipe.Title = updatedRecipe.Title;
            recipe.Description = updatedRecipe.Description;
            recipe.Ingredients = updatedRecipe.Ingredients;
            recipe.Procedure = updatedRecipe.Procedure;
            recipe.IsFree = updatedRecipe.IsFree;

            await _context.SaveChangesAsync();
            return Ok("Recipe updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/Recipe/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        try
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return Ok("Recipe deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
