using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace ArtcordAdminBot.Listeners
{
    public class ButtonInteractionListener
    {

        private readonly IDatabaseService _databaseService;

        public ButtonInteractionListener(IDatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public async Task HandleButtonInteraction(DiscordClient client, ComponentInteractionCreatedEventArgs e)
        {
            if (e.Id == "claim_ticket")
            {
                var (ticketId, userId) = await _databaseService.GetTicketIdForChannelAsync(e.Channel.Id);

                if (!ticketId.HasValue)
                {
                    await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("Ticket not found.").AsEphemeral(true));
                    return;
                }

                bool isClaimed = await _databaseService.GetTicketClaimedStatusAsync(ticketId.Value);

                if (isClaimed)
                {
                    await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("This ticket has already been claimed.").AsEphemeral(true));
                    return;
                }

                await _databaseService.SetTicketClaimedStatusAsync(ticketId.Value, e.User.Id, true);

                var originalMessage = await e.Channel.GetMessageAsync(e.Message.Id);
                
                var embed = new DiscordEmbedBuilder
                {
                    Title = originalMessage.Embeds[0].Title,
                    Description = originalMessage.Embeds[0].Description + $"\n\n**This ticket has been claimed by:** {e.User.Mention}",
                    Color = originalMessage.Embeds[0].Color
                };

                await originalMessage.ModifyAsync(msg =>
                {
                    msg.RemoveEmbedAt(0);
                    msg.AddEmbed(embed.Build());
                });

                await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.DeferredMessageUpdate);
            }


            else if (e.Id == "close_ticket")
            {
                await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("This ticket is now closed."));
                
                var (ticketId, userId) = await _databaseService.GetTicketIdForChannelAsync(e.Channel.Id);

                await _databaseService.closeTicketAsync(ticketId: ticketId!.Value);
                await e.Channel.DeleteAsync();
            }

            else if (e.Id == "appeal_ban")
            {
                await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Appeal sent."));

            }
            
        }

    }
}
