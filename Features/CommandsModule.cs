using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using ArtcordAdminBot.Database;

namespace ArtcordAdminBot.Features
{
    // CommandsModule class contains the slash commands for the Discord bot.
    public class CommandsModule : ApplicationCommandModule
    {
        // PingCommand is a slash command that responds with "Pong!" when invoked.
        [SlashCommand("ping", "Responds with Pong!")]
        public async Task PingCommand(InteractionContext ctx,
            // The 'embed' option determines if the response should be an embed. Default is false.
            [Option("embed", "Should the response be an embed?")] bool embed = false)
        {
            // Log the command
            await DatabaseHelper.LogCommandAsync(ctx.User.Id.ToString(), ctx.User.Username, "ping", null);

            // If the 'embed' option is true, send an embedded response.
            if (embed)
            {
                // Create an embed with the title "Pong!" and a green color.
                var embedResponse = new DiscordEmbedBuilder
                {
                    Title = "Pong!",
                    Color = DiscordColor.Green
                };

                // Send the embed as the response to the slash command.
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(embedResponse));
            }
            else 
            {
                // If 'embed' is false, send a simple text response with "Pong!".
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Pong!"));
            }
        }

        // EchoCommand is a slash command that repeats the provided message when invoked.
        [SlashCommand("echo", "Repeats the message provided.")]
        public async Task EchoCommand(InteractionContext ctx,
            // The 'message' option is the text that will be repeated by the bot.
            [Option("message", "The message to repeat")] string message,
            // The 'embed' option determines if the response should be an embed. Default is false.
            [Option("embed", "Should the response be an embed?")] bool embed = false)
        {
            // Log the command
            await DatabaseHelper.LogCommandAsync(ctx.User.Id.ToString(), ctx.User.Username, "echo", null);

            // If the 'embed' option is true, send an embedded response.
            if (embed)
            {
                // Create an embed with the title "Echo" and the message as the description.
                var embedResponse = new DiscordEmbedBuilder
                {
                    Title = "Echo",
                    Description = message,
                    Color = DiscordColor.Blurple
                };

                // Send the embed as the response to the slash command.
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(embedResponse));
            }
            else
            {
                // If 'embed' is false, send the message as a simple text response.
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent(message));
            }
        }
    }
}
