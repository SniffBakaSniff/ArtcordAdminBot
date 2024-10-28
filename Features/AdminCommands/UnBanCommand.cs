using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Commands.ContextChecks;
using ArtcordAdminBot.Features.Helpers;

namespace ArtcordAdminBot.Features.AdminCommands
{
    public partial class AdminCommandGroup
    {
        

        [Command("Unban")]
        [RequirePermissions(DiscordPermissions.BanMembers)]
        public async Task BanAsync(CommandContext ctx, 
        [System.ComponentModel.Description("The user to unban.")]ulong targetUser, 
        [System.ComponentModel.Description("The reason for the unban.")] string? reason = null)
        {
            
            // Place DB logic here for unbans
            //await _databaseService.NewBanRecordAsync();

            await ctx.Guild!.UnbanMemberAsync(targetUser, reason);

            var embed = new DiscordEmbedBuilder
            {
                Title = "User UnBanned",
                Color = DiscordColor.Cyan,
                Description = $"**User:** {targetUser}\n" +
                              $"**Reason:** {reason}\n" +
                              $"**Moderator:** {ctx.User.Username}",
                Timestamp = DateTime.UtcNow
            };

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
