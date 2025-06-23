using Microsoft.EntityFrameworkCore;
using Wordle.Data.Models;

namespace Wordle.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
            
        }

        public DbSet<AllWord> AllWords { get; set; }

    }
}
