using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; } // Admin, Member, Guest
}
