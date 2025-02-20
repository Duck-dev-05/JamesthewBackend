public class Recipe
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Ingredients { get; set; }
    public required string Procedure { get; set; }
    public bool IsFree { get; set; }
    public required string UploadedBy { get; set; }
}
