using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JamesThew.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IdentityDbContext<ApplicationUser>
    {
        public required DbSet<Recipe> Recipes { get; set; }
        public required DbSet<Contest> Contests { get; set; }
        public required DbSet<Feedback> Feedbacks { get; set; }
        public required DbSet<FAQ> FAQs { get; set; }
    }
}
