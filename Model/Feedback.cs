namespace JamesthewBackend.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Message { get; set; }
        public DateTime DateSubmitted { get; set; }
    }
}
