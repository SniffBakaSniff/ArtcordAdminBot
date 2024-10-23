using Microsoft.EntityFrameworkCore;

public class BotDbContext : DbContext
{
    public DbSet<GuildSettings> GuildSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your model
        base.OnModelCreating(modelBuilder);
    }
}
