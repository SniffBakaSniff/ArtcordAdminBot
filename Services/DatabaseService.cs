using Microsoft.EntityFrameworkCore;


public interface IDatabaseService
{
    Task<ulong?> GetLogsChannelAsync(ulong guildId);
    Task<string> GetPrefixAsync(ulong guildId);
    Task SetPrefixAsync(ulong guildId, string prefix);
    Task<ulong?> GetMutedRoleAsync(ulong guildId);
    Task SetMutedRoleAsync(ulong guildId, ulong? mutedRoleId);
    Task<(ulong? TicketId, ulong? UserId)> GetTicketIdForChannelAsync(ulong channelId);
    Task<ulong?> GetTicketCountAsync(ulong guildId, ulong? userId = null);
    Task<bool> GetTicketClaimedStatusAsync(ulong ticketId);
    Task SetTicketClaimedStatusAsync(ulong ticketId, ulong userId ,bool claimed);
    Task LogTicketMessageAsync(ulong ticketId, ulong userId, TicketMessageType messageType, string content);
    Task closeTicketAsync(ulong ticketId);
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
    Task NewTicketRecordAsync(
        ulong guildId,
        ulong channelId,
        ulong userId,
        string reason,
        DateTime openedAt,
        DateTime? closedAt = null);

    Task<string?> ManageMessageSettingAsync(ulong guildId, string messageType, string? newMessage = null);
    Task<ulong?> GetWelcomeChannelAsync(ulong guildId);
    Task SetWelcomeChannelAsync(ulong guildId, ulong? welcomeChannelId);
    Task<ulong?> GetFarewellChannelAsync(ulong guildId);
    Task SetFarewellChannelAsync(ulong guildId, ulong? farewellChannelId);
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
    private async Task<GuildMessageSettings> GuildMessageSettingsAsync(BotDbContext dbContext, ulong guildId)
    {
        var settings = await dbContext.GuildMessageSettings.FindAsync(guildId);

        if (settings == null)
        {
            settings = new GuildMessageSettings { GuildId = guildId };
            dbContext.GuildMessageSettings.Add(settings);
        }

        return settings;
    }

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

    public async Task<(ulong? TicketId, ulong? UserId)> GetTicketIdForChannelAsync(ulong channelId)
    {
        using (var dbContext = new BotDbContext())
        {
            var ticket = await dbContext.TicketRecords
                .FirstOrDefaultAsync(t => t.ChannelId == channelId);

            return (ticket?.Id, ticket?.UserId);
        }
    }

    public async Task<ulong?> GetTicketCountAsync(ulong guildId, ulong? userId = null)
    {
        using (var dbContext = new BotDbContext())
        {
            return (ulong?)await dbContext.TicketRecords
                .Where(t => t.GuildId == guildId && (userId == null || t.UserId == userId))
                .CountAsync();
        }
    }

    public async Task<bool> GetTicketClaimedStatusAsync(ulong ticketId)
    {
        using (var dbContext = new BotDbContext())
        {
            return await dbContext.TicketRecords
                .AnyAsync(t => t.Id == ticketId && t.IsClaimed);
        }
    }

    public async Task SetTicketClaimedStatusAsync(ulong ticketId, ulong userId ,bool isClaimed)
    {
        using (var dbContext = new BotDbContext())
        {
            var ticket = await dbContext.TicketRecords
                .FirstOrDefaultAsync(t => t.Id == ticketId);
            ticket!.IsClaimed = isClaimed;
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task LogTicketMessageAsync(ulong ticketId, ulong userId, TicketMessageType messageType, string content)
    {
        using (var dbContext = new BotDbContext())
        {
            var newMessage = new TicketMessages
            {
                TicketId = ticketId,
                UserId = userId,
                MessageType = messageType,
                Content = content
            };

            dbContext.TicketMessages.Add(newMessage);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task closeTicketAsync(ulong ticketId)
    {
        using (var dbContext = new BotDbContext())
        {
            var ticket = await dbContext.TicketRecords.FindAsync(ticketId);
            ticket!.ClosedAt = DateTime.UtcNow;
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

    public async Task NewTicketRecordAsync(
        ulong guildId,
        ulong channelId,
        ulong userId,
        string reason,
        DateTime openedAt,
        DateTime? closedAt = null)
    {
        using (var dbContext = new BotDbContext())
        {
            var newTicketRecord = new TicketRecords
            {
                GuildId = guildId,
                ChannelId = channelId,
                UserId = userId,
                OpenedAt = openedAt = DateTime.UtcNow,
                ClosedAt = closedAt,
                Reason = reason
            };

            dbContext.TicketRecords.Add(newTicketRecord);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<string?> ManageMessageSettingAsync(ulong guildId, string messageType, string? newMessage = null)
    {
        using (var dbContext = new BotDbContext())
        {
            var settings = await GuildMessageSettingsAsync(dbContext, guildId);

            switch (messageType.ToLower())
            {
                case "appeal":
                    if (newMessage != null)
                    {
                        settings.AppealMessage = newMessage;
                        await dbContext.SaveChangesAsync();
                    }
                    return settings.AppealMessage;

                case "welcome":
                    if (newMessage != null)
                    {
                        settings.WelcomeMessage = newMessage;
                        await dbContext.SaveChangesAsync();
                    }
                    return settings.WelcomeMessage;

                case "farewell":
                    if (newMessage != null)
                    {
                        settings.FarewellMessage = newMessage;
                        await dbContext.SaveChangesAsync();
                    }
                    return settings.FarewellMessage;

                case "mutednotification":
                    if (newMessage != null)
                    {
                        settings.MutedNotificationMessage = newMessage;
                        await dbContext.SaveChangesAsync();
                    }
                    return settings.MutedNotificationMessage;

                case "ticketcreation":
                    if (newMessage != null)
                    {
                        settings.TicketCreationMessage = newMessage;
                        await dbContext.SaveChangesAsync();
                    }
                    return settings.TicketCreationMessage;

                case "ticketclosure":
                    if (newMessage != null)
                    {
                        settings.TicketClosureMessage = newMessage;
                        await dbContext.SaveChangesAsync();
                    }
                    return settings.TicketClosureMessage;
                    

                case "error":
                    if (newMessage != null)
                    {
                        settings.ErrorMessage = newMessage;
                    }
                    return settings.ErrorMessage;

                default:
                    throw new ArgumentException("Invalid message type specified.");
            }
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
}

