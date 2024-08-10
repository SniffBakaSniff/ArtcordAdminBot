using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using ArtcordAdminBot.Database;
using System.Text;

namespace ArtcordAdminBot.Features
{
    public class CommandLogModule : ApplicationCommandModule
    {
        [SlashCommand("viewlogs", "Displays the command logs.")]
        public async Task ViewLogsCommand(InteractionContext ctx)
        {
            await DatabaseHelper.LogCommandAsync(ctx.User.Id.ToString(), ctx.User.Username, "viewlogs", null);


            // Retrieve the logs from the database
            var logs = await DatabaseHelper.GetCommandLogsAsync();

            // Format the logs into a readable message
            var sb = new StringBuilder();
            sb.AppendLine("**Command Logs**");
            sb.AppendLine("```");

            foreach (var log in logs)
            {
                sb.AppendLine($"[{log.Timestamp}] {log.UserName} ({log.UserId}) executed {log.CommandName} {log.CommandArgs}");
            }

            sb.AppendLine("```");

            // Send the logs as a response
            var embed = new DiscordEmbedBuilder
            {
                Title = "Command Logs",
                Description = sb.ToString(),
                Color = DiscordColor.Blurple
            };

            await ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder()
                .AddEmbed(embed));
        }
    }
}
