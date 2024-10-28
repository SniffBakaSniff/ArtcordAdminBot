using ArtcordAdminBot.Features.Helpers;
using DSharpPlus.Commands;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Features.AdminCommands
{
    public partial class AdminCommandGroup
    {
        [Command("unmute")]
        [System.ComponentModel.Description("Unmutes a user")]
        public async Task UnMuteAsync(CommandContext ctx,
        [System.ComponentModel.Description("The user to Unmute.")] DiscordUser targetUser)
        {
            var mutedRoleId = await _databaseService.GetMutedRoleAsync(ctx.Guild!.Id);
            if (mutedRoleId == null)
            {
                await ctx.RespondAsync(MessageHelpers.GenericErrorEmbed("Muted role not set. Please configure the muted role with `/config setmutedrole`."));
                return;
            }

            var mutedRole = ctx.Guild.GetRole(mutedRoleId.Value);
            if (mutedRole is null)
            {
                await ctx.RespondAsync(MessageHelpers.GenericErrorEmbed("Muted role not found in the guild."));
                return;
            }

            var targetMember = await ctx.Guild.GetMemberAsync(targetUser.Id);
            await targetMember.RevokeRoleAsync(mutedRole);

            await ctx.RespondAsync(MessageHelpers.GenericSuccessEmbed($"User Unmuted", $"{targetUser.Mention} has been unmuted."));
        }
    }
}
