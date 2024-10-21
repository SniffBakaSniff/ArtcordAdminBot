using Microsoft.EntityFrameworkCore;

public interface IDatabaseService
{
    Task<string> GetPrefixAsync(ulong guildId);
    Task SetPrefixAsync(ulong guildId, string prefix);
}

public class DatabaseService : IDatabaseService
{
    private readonly BotDbContext _context;

    public DatabaseService(BotDbContext context)
    {
        _context = context;
    }

    //This is for getting the prefix
    public async Task<string> GetPrefixAsync(ulong guildId)
    {
        var settings = await _context.GuildSettings.FindAsync(guildId);
        return settings?.Prefix ?? "!";
    }

    //This is for changing the prefix
    public async Task SetPrefixAsync(ulong guildId, string prefix)
    {
        var settings = await _context.GuildSettings.FindAsync(guildId);

        if (settings == null)
        {
            settings = new GuildSettings { GuildId = guildId, Prefix = prefix };
            _context.GuildSettings.Add(settings);
        }
        else
        {
            settings.Prefix = prefix;
        }

        await _context.SaveChangesAsync();
    }

}
