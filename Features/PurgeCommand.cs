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
        [System.ComponentModel.Description("Purges a specified number of messages or until a certain message is reached from the channel.")]
        public static async Task PurgeAsync(CommandContext context, 
            [System.ComponentModel.Description("The amount of messages to purge from the channel (1 to 250, default 10)")] int amount = -1,
            [System.ComponentModel.Description("The message link or ID to delete after. It itself will not get deleted.")] string? afterMessage = null)
        {
            try
            {
                bool useAmount = string.IsNullOrEmpty(afterMessage);

                if (useAmount)
                {
                    if (amount == -1)
                        amount = 10;
                }
                else if (amount != -1)
                {
                    await context.RespondAsync(
                        MessageHelpers.GenericErrorEmbed("Argument `amount` cannot be provided if `untilMessage` is provided.")
                        );
                    return;
                }

                // Ensure the count is within range
                if (useAmount && (amount <= 1 || amount > 250))
                {
                    await context.RespondAsync(
                        MessageHelpers.GenericErrorEmbed("A number from **1** to **250** must be provided for the parameter `amount`.")
                    );
                    return;
                }

                // Collect messages into a list
                var messages = new List<DiscordMessage>();
                var cancellationToken = CancellationToken.None;

                if (useAmount)
                {
                    await foreach (var message in context.Channel.GetMessagesAsync(amount, cancellationToken))
                    {
                        messages.Add(message);
                    }
                }
                else
                {
                    await foreach (var message in context.Channel.GetMessagesAfterAsync(ConversionHelpers.GetMessageId(afterMessage, context)))
                    {
                        messages.Add(message);
                    }
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
