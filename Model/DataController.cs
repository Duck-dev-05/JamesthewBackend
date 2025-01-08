using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Hello from ASP.NET Core Backend!" });
    }
}
