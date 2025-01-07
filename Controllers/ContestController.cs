using JamesthewBackend.Data;
using JamesthewBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace JamesthewBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddContestEntry([FromBody] ContestEntry entry)
        {
            if (entry == null) return BadRequest("Invalid entry");

            _context.ContestEntries.Add(entry);
            _context.SaveChanges();

            return Ok(entry);
        }
    }
}
