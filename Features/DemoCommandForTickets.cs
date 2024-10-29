using DSharpPlus.Commands;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Threading.Tasks;

public class TicketCommandDemo  
{
    private readonly IDatabaseService _databaseService;

    public TicketCommandDemo(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [Command("ticketmessages")]
    [Description("Checks the number of messages sent in a specific ticket.")]
    public async Task TicketMessagesCommand(CommandContext ctx, DiscordChannel ticket)
    {
        // Get the ticket ID for the given channel
        (ulong? TicketId, ulong? UserId) = await _databaseService.GetTicketIdForChannelAsync(ticket.Id);

        if (!TicketId.HasValue)
        {
            await ctx.RespondAsync("Could not find a ticket for this channel.");
            return;
        }

        // Get the message count for the specified ticket ID
        var messageCount = await _databaseService.GetMessageCountForTicketAsync(TicketId.Value);

        await ctx.RespondAsync($"The ticket with ID {TicketId.Value} has {messageCount} messages.");
    }
}
