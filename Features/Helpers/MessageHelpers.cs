using DSharpPlus.Entities;
using System.Threading.Channels;

namespace ArtcordAdminBot.Features.Helpers
{
    public class MessageHelpers
    {
        public static DiscordEmbed GenericSuccessEmbed(string title, string message) =>
            GenericEmbed(title, message, "#20c020");

        public static DiscordEmbed GenericErrorEmbed(string message) =>
            GenericEmbed("Error", message, "#ff0000");

        public static DiscordEmbed GenericEmbed(string title, string message, string color = "#5865f2") => new DiscordEmbedBuilder()
                .WithTitle(title)
                .WithColor(new DiscordColor(color))
                .WithDescription(message)
                .WithTimestamp(DateTime.UtcNow)
                .Build();
    }
}

