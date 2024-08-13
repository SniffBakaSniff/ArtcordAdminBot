using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArtcordAdminBot.Features.Helpers;

namespace ArtcordAdminBot.Features
{
    public class PurgeCommand
    {
        [Command("purge")]
        [System.ComponentModel.Description("Purges a specified number of messages from the channel. Defaults to 10 if no number is specified.")]
        public static async Task PurgeMessagesAsync(CommandContext context, [System.ComponentModel.Description("The amount of messages to purge from the channel (1 to 250)")] int amount = 10)
        {
            try
            {
                // Ensure the count is within range
                if (amount <= 1 || amount > 250)
                {
                    await context.RespondAsync(
                        MessageHelpers.GenericErrorEmbed("A number from **1** to **250** must be provided for the parameter `amount`.")
                    );
                    return;
                }

                // Collect messages into a list
                var messages = new List<DiscordMessage>();
                var cancellationToken = CancellationToken.None;

                await foreach (var message in context.Channel.GetMessagesAsync(amount, cancellationToken))
                {
                    messages.Add(message);
                }

                // Bulk delete messages
                if (messages.Count > 0)
                {
                    await context.Channel.DeleteMessagesAsync(messages);
                }
                else
                {
                    await context.RespondAsync(
                        MessageHelpers.GenericErrorEmbed("No messages to delete")
                        );
                }

                // Send a confirmation embed
                await context.RespondAsync(
                        MessageHelpers.GenericSuccessEmbed("Purge complete!", $"Successfully deleted {messages.Count} message{(messages.Count == 1 ? "" : "s")}.")
                    );
                return;
            }
            catch (Exception ex)
            {
                await context.RespondAsync(
                    MessageHelpers.GenericErrorEmbed($"An error occurred while purging messages: {ex.Message}")
                );
            }
        }
    }
}
