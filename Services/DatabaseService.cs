using Microsoft.EntityFrameworkCore;

public interface IDatabaseService
{
    Task<string> GetPrefixAsync(ulong guildId);
    Task SetPrefixAsync(ulong guildId, string prefix);
    Task<ulong?> GetMutedRoleAsync(ulong guildId);
    Task SetMutedRoleAsync(ulong guildId, ulong? mutedRoleId);
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

    private async Task<GuildSettings> GuildSettingsAsync(BotDbContext dbContext, ulong guildId)
    {
        var settings = await dbContext.GuildSettings.FindAsync(guildId);

        if (settings == null)
        {
            settings = new GuildSettings { GuildId = guildId };
            dbContext.GuildSettings.Add(settings);
        }

        return settings;
    }


    public async Task<string> GetPrefixAsync(ulong guildId)
    {
        using (var dbContext = new BotDbContext())
        {
            var settings = await dbContext.GuildSettings.FindAsync(guildId);
            return settings?.Prefix ?? "!";
        }
    }

    public async Task SetPrefixAsync(ulong guildId, string prefix)
    {
        using (var dbContext = new BotDbContext())
        {
            var settings = await GuildSettingsAsync(dbContext, guildId);
            settings.Prefix = prefix;
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<ulong?> GetMutedRoleAsync(ulong guildId)
    {
        using (var dbContext = new BotDbContext())
        {
            var settings = await dbContext.GuildSettings.FindAsync(guildId);
            return settings?.MutedRoleId;
        }
    }

    public async Task SetMutedRoleAsync(ulong guildId, ulong? mutedRoleId)
    {
        using (var dbContext = new BotDbContext())
        {
            var settings = await GuildSettingsAsync(dbContext, guildId);
            settings.MutedRoleId = mutedRoleId;
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

