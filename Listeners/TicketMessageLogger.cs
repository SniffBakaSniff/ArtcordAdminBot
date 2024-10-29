using DSharpPlus;
using DSharpPlus.EventArgs;

namespace ArtcordAdminBot.Listeners
{
    public class TicketMessageLogger
    {

        private readonly IDatabaseService _database;

        public TicketMessageLogger(IDatabaseService database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }
       
        public async Task LogTicketMessages(DiscordClient client, MessageCreatedEventArgs e)
        {
            var (ticketId, userId) = await _database.GetTicketIdForChannelAsync(e.Channel.Id);
            var messageType = e.Author.Id == userId ? TicketMessageType.Moderator : TicketMessageType.Submitter;
            if(ticketId.HasValue) 
            {
                Console.WriteLine(e.Message.Content);
                await _database.LogTicketMessageAsync(ticketId: ticketId.Value, userId: e.Author.Id, messageType: messageType, content: e.Message.Content);
            }
        }
    }
}
