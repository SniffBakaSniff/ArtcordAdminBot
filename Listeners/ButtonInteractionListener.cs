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
                    Color = DiscordColor.Red // You can choose any color
                }
                .AddField("Action Required", "Click the button below to confirm the closure of this ticket.", true);

                await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .AddEmbed(embed.Build()) // Add the embed to the response
                        .AddComponents(
                            new DiscordButtonComponent(DiscordButtonStyle.Danger, "close_ticket_confirmation", "Confirm")
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
        }
    }
}
