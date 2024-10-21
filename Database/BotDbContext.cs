using Microsoft.EntityFrameworkCore;

public class BotDbContext : DbContext
{
    public DbSet<GuildSettings> GuildSettings { get; set; }

    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure if options are not already set
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Database.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your model
        base.OnModelCreating(modelBuilder);
    }
}
