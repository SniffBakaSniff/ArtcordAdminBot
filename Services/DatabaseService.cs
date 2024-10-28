using Microsoft.EntityFrameworkCore;

public interface IDatabaseService
{
    Task<string> GetPrefixAsync(ulong guildId);
    Task SetPrefixAsync(ulong guildId, string prefix);
    Task NewBanRecordAsync(
        ulong guildId,
        ulong userId,
        ulong moderatorId,
        string? reason = null,
        string? referenceImagePath = null,
        ulong? referenceMessageId = null,
        DateTime? expirationDate = null,
        AppealStatus? appealStatus = null,
        DateTime? appealDate = null,
        string? internalNotes = null);
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

    public async Task NewBanRecordAsync(
        ulong guildId,
        ulong userId,
        ulong moderatorId,
        string? reason = null,
        string? referenceImagePath = null,
        ulong? referenceMessageId = null,
        DateTime? expirationDate = null,
        AppealStatus? appealStatus = null,
        DateTime? appealDate = null,
        string? internalNotes = null)
    {
        using (var dbContext = new BotDbContext())
        {
            var newBanRecord = new BanRecords
            {
                GuildId = guildId,
                UserId = userId,
                ModeratorId = moderatorId,
                Reason = reason,
                ReferenceImagePath = referenceImagePath,
                ReferenceMessageId = referenceMessageId,
                BanDate = DateTime.UtcNow,
                ExpirationDate = expirationDate,
                AppealStatus = appealStatus,
                AppealDate = appealDate,
                InternalNotes = internalNotes
            };

            dbContext.BanRecords.Add(newBanRecord);
            await dbContext.SaveChangesAsync();
        }
    }

}

