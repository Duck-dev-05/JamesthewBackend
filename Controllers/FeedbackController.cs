using JamesthewBackend.Data;
using JamesthewBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace JamesthewBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SubmitFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null) return BadRequest("Invalid feedback");

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            return Ok(feedback);
        }
    }
}
