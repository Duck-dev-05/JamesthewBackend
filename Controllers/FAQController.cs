using JamesthewBackend.Data;
using JamesthewBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace JamesthewBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FAQController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FAQController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetFAQs()
        {
            return Ok(_context.FAQs.ToList());
        }
    }
}
