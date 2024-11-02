
using Microsoft.EntityFrameworkCore;

namespace ArtcordAdminBot.Services.Database
{

    public class TicketService : ITicketService
    {
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

        public async Task CloseTicketAsync(ulong ticketId)
        {
            using (var dbContext = new BotDbContext())
            {
                var ticket = await dbContext.TicketRecords.FindAsync(ticketId);
                ticket!.ClosedAt = DateTime.UtcNow;
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
    }
}