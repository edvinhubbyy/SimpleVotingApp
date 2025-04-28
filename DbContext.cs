using Microsoft.EntityFrameworkCore;
using SimpleVotingApp.Models;
using System.Linq;

public class VotingContext : DbContext
{
    public VotingContext(DbContextOptions<VotingContext> options) : base(options) { }

    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Vote> Votes { get; set; }

    // Seed method accepts VotingContext as parameter
    public static void SeedDatabase(VotingContext context)
    {
        if (!context.Candidates.Any())
        {
            
            context.SaveChanges();
        }
    }
}


