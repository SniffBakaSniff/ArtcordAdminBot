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
        public static async Task PurgeMessagesAsync(CommandContext context, int amount = 10)
        {
            try
            {
                // Ensure the count is positive and within a reasonable range (e.g., up to 250)
                if (amount <= 0 || amount > 250)
                {
                    await context.RespondAsync(
                        MessageHelpers.GenericErrorEmbed("Please specify a positive number up to 250 for the number of messages to delete.")
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

                // Send a confirmation embed
                await context.RespondAsync(
                        MessageHelpers.GenericEmbed("Purge Complete!", $"Successfully deleted {messages.Count} messages.")
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
