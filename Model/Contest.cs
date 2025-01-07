using JamesthewBackend.Models;

public class Contest
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required ICollection<ContestEntry> Entries { get; set; } // Submissions to the contest
}
