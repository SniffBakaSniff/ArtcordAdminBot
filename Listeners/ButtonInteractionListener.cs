using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace ArtcordAdminBot.Listeners
{
    public class ButtonInteractionListener
    {

        private readonly ITicketService _ticketService;


        public ButtonInteractionListener(ITicketService ticketService)
        {
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
        }

        public async Task HandleButtonInteraction(DiscordClient client, ComponentInteractionCreatedEventArgs e)
        {
            if (e.Id == "claim_ticket")
            {
                var (ticketId, userId) = await _ticketService.GetTicketIdForChannelAsync(e.Channel.Id);

                if (!ticketId.HasValue)
                {
                    await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("Ticket not found.").AsEphemeral(true));
                    return;
                }

                bool isClaimed = await _ticketService.GetTicketClaimedStatusAsync(ticketId.Value);

                if (isClaimed)
                {
                    await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("This ticket has already been claimed.").AsEphemeral(true));
                    return;
                }

                var member = await e.Guild.GetMemberAsync(e.User.Id);
                if (!member.Permissions.HasPermission(DiscordPermissions.ManageMessages)) //!member.Roles.Any(role => role.id == ModeratorRoles)) Demo method for the custom Role Groups for Permission handling
                                                                                          //Yes Ik this logic is incorrect and wouldnt work but its just a idea i had.
                {
                    await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("You do not have permission to claim this ticket.").AsEphemeral(true));
                    return;
                }

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
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Close Ticket",
                    Description = "Are you sure you want to close this ticket?",
                    Color = DiscordColor.Red
                }.WithFooter("This action cannot be undone.");

                await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .AddEmbed(embed.Build())
                        .AddComponents(
                            new DiscordButtonComponent(DiscordButtonStyle.Danger, "close_ticket_confirmation", "Confirm"),
                            new DiscordButtonComponent(DiscordButtonStyle.Secondary, "close_ticket_cancellation", "Cancel")
                        )
                        .AsEphemeral(true));
            }


            else if (e.Id == "appeal_ban")
            {
                await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Appeal sent."));

            }
            else if (e.Id == "close_ticket_confirmation")
            {
                await e.Channel.DeleteAsync();
            }
            else if (e.Id == "close_ticket_cancellation")
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Close Ticket",
                    Description = "Ticket closure has been canceled.",
                    Color = DiscordColor.Green
                };

                await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder()
                        .AddEmbed(embed.Build())
                        .AddComponents(
                            new DiscordButtonComponent(DiscordButtonStyle.Danger, "close_ticket_confirmation", "Confirm", disabled: true),
                            new DiscordButtonComponent(DiscordButtonStyle.Secondary, "close_ticket_cancellation", "Cancel", disabled: true)
                        )
                        .AsEphemeral(true));
            }
        }
    }
}
