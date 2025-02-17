using ChatBot.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<History> History { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<History>().ToTable("history");
    }
}