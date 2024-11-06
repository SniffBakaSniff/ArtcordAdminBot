using DSharpPlus.Commands;
using DSharpPlus.Entities;

namespace ArtcordBot.Features
{
    public class PingCommand
    {
        private static readonly DateTime _botStartTime = DateTime.UtcNow;

        [Command("ping")]
        public async Task PingAsync(CommandContext ctx)
        {
            var latency = ctx.Client.GetConnectionLatency(ctx.Guild!.Id);
            var roundedLatency = Math.Round(latency.TotalMilliseconds);
            var uptime = DateTime.UtcNow - _botStartTime;
            var embed = new DiscordEmbedBuilder()
                .WithTitle("🏓 Pong!")
                .AddField("Latency", $"{roundedLatency} ms", true)
                .AddField("Bot Uptime", $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m", true)
                .WithColor(DiscordColor.Cyan)
                .WithTimestamp(DateTime.UtcNow);

            await ctx.RespondAsync(embed.Build());
        }
    }
}