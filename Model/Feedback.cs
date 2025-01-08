public class Feedback
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required string RecipeId { get; set; }
    public required string Comments { get; set; }
}
