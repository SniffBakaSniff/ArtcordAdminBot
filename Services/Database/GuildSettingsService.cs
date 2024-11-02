namespace ArtcordAdminBot.Services.Database
{
    public class GuildSettingsService : IGuildSettingsService
    {
        public async Task<ulong?> GetLogsChannelAsync(ulong guildId)
        {
            using (var dbContext = new BotDbContext())
            {
                var settings = await dbContext.GuildSettings.FindAsync(guildId);
                return settings?.LogsChannelId;
            }
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

        public async Task<ulong?> GetWelcomeChannelAsync(ulong guildId)
        {
            using (var dbContext = new BotDbContext())
            {
                var settings = await GuildSettingsAsync(dbContext, guildId);
                return settings?.WelcomeChannelId;
            }
        }

        public async Task SetWelcomeChannelAsync(ulong guildId, ulong? welcomeChannelId)
        {
            using (var dbContext = new BotDbContext())
            {
                var settings = await GuildSettingsAsync(dbContext, guildId);
                settings!.WelcomeChannelId = welcomeChannelId;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<ulong?> GetFarewellChannelAsync(ulong guildId)
        {
            using (var dbContext = new BotDbContext())
            {
                var settings = await GuildSettingsAsync(dbContext, guildId);
                return settings?.FarewellChannelId;
            }
        }

        public async Task SetFarewellChannelAsync(ulong guildId, ulong? farewellChannelId)
        {
            using (var dbContext = new BotDbContext())
            {
                var settings = await GuildSettingsAsync(dbContext, guildId);
                settings!.FarewellChannelId = farewellChannelId;
                await dbContext.SaveChangesAsync();
            }
        }

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
    }
}