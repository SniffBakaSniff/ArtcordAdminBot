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
    }
}
