namespace JamesthewBackend.Models
{
    public class ContestEntry
    {
        public int Id { get; set; }
        public required string ContestName { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
