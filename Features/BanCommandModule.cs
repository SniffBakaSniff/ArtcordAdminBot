using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using ArtcordAdminBot.Database;

namespace ArtcordAdminBot.Features
{
    public class BanCommandsModule : ApplicationCommandModule
    {
        [SlashCommand("ban", "Bans a user!")]
        public async Task BanCommand(InteractionContext ctx,
            [Option("user", "The user to ban")] DiscordUser user)
        {
            await DatabaseHelper.LogCommandAsync(ctx.User.Id.ToString(), "ban");

            // Create an embed response showing the user being banned
            var embedResponse = new DiscordEmbedBuilder
            {
                Title = "Banned",
                Description = $"User: {user.Username}#{user.Discriminator} (ID: {user.Id})",
                Color = DiscordColor.Blurple
            };

            // Send the response with the embed
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embedResponse));
        }

        [SlashCommand("unban", "Un-bans a user.")]
        public async Task UnbanCommand(InteractionContext ctx,
            [Option("user", "The user to unban")] DiscordUser user)
        {
            await DatabaseHelper.LogCommandAsync(ctx.User.Id.ToString(), "unban");

            // Create an embed response showing the user being unbanned
            var embedResponse = new DiscordEmbedBuilder
            {
                Title = "Un-Banned",
                Description = $"User: {user.Username} (ID: {user.Id})",
                Color = DiscordColor.Blurple
            };

            // Send the response with the embed
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embedResponse));
        }
    }
}
