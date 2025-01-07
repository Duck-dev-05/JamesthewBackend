using JamesthewBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace JamesthewBackend.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public required DbSet<ContestEntry> ContestEntries { get; set; }
        public required DbSet<FAQ> FAQs { get; set; }
        public required DbSet<Feedback> Feedbacks { get; set; }
        
        public required DbSet<Recipe> Recipes {get;set;}

        public required DbSet<User> users {get;set;}
    }
}
