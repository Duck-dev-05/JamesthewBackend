using JamesthewBackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public UserController(AppDbContext context, IConfiguration config)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto registerDto)
    {
        if (registerDto == null || string.IsNullOrEmpty(registerDto.Username) ||
            string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password))
        {
            return BadRequest(new { Message = "Invalid registration data provided." });
        }

        if (_context.users.Any(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
        {
            return Conflict(new { Message = "Username or Email already exists." });
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = hashedPassword,
            Role = "Guest"
        };

        _context.users.Add(user);
        _context.SaveChanges();

        return Ok(new { Message = "User registered successfully." });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
        {
            return BadRequest(new { Message = "Invalid login data provided." });
        }

        var user = _context.users.SingleOrDefault(u => u.Username == loginDto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = _config["Jwt:Key"];
        var issuer = _config["Jwt:Issuer"];

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer))
        {
            throw new InvalidOperationException("JWT configuration is missing in the app settings.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
