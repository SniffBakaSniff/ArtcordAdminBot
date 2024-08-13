using DSharpPlus.Entities;
using System.Threading.Channels;

namespace ArtcordAdminBot.Features.Helpers
{
    public class MessageHelpers
    {
        public static DiscordEmbed GenericSuccessEmbed(string message) => new DiscordEmbedBuilder()
            .WithTitle("Success")
            .WithColor(new DiscordColor("#20c020"))
            .WithDescription(message)
            .WithTimestamp(DateTime.UtcNow)
            .Build();

        public static DiscordEmbed GenericErrorEmbed(string message) => new DiscordEmbedBuilder()
                .WithTitle("Error")
                .WithColor(new DiscordColor("#ff0000"))
                .WithDescription(message)
                .WithTimestamp(DateTime.UtcNow)
                .Build();

        public static DiscordEmbed GenericEmbed(string title, string message, string color = "#20c020") => new DiscordEmbedBuilder()
                .WithTitle(title)
                .WithColor(new DiscordColor(color))
                .WithDescription(message)
                .WithTimestamp(DateTime.UtcNow)
                .Build();
    }
}

