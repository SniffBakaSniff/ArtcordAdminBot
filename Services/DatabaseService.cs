using Microsoft.EntityFrameworkCore;

public interface IDatabaseService
{
    Task<string> GetPrefixAsync(ulong guildId);
    Task SetPrefixAsync(ulong guildId, string prefix);
}

public class DatabaseService : IDatabaseService
{
    // This is for getting the prefix
    public async Task<string> GetPrefixAsync(ulong guildId)
    {
        using (var dbContext = new BotDbContext())
        {
            var settings = await dbContext.GuildSettings.FindAsync(guildId);
            return settings?.Prefix ?? "!";
        }
    }

    // This is for changing the prefix
    public async Task SetPrefixAsync(ulong guildId, string prefix)
    {
        using (var dbContext = new BotDbContext())
        {
            var settings = await dbContext.GuildSettings.FindAsync(guildId);

            if (settings == null)
            {
                settings = new GuildSettings { GuildId = guildId, Prefix = prefix };
                dbContext.GuildSettings.Add(settings);
            }
            else
            {
                settings.Prefix = prefix;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
