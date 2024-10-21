using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Commands.ContextChecks;
using ArtcordAdminBot.Features.Helpers;


namespace ArtcordAdminBot.Features
{
    public class PrefixCommand
    {
        private readonly IDatabaseService _databaseService;

        public PrefixCommand(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [Command("setprefix")]
        [System.ComponentModel.Description("Set the bot's prefix")]
        //[RequirePermissions(DiscordPermissions.Administrator)]
        public async Task SetPrefixAsync(CommandContext ctx, string newPrefix)
        {
            if (string.IsNullOrWhiteSpace(newPrefix))
            {
                await ctx.RespondAsync("Prefix cannot be empty.");
                return;
            }

            await _databaseService.SetPrefixAsync(ctx.Guild.Id, newPrefix);

            await ctx.RespondAsync(
                MessageHelpers.GenericEmbed("",$"Prefix successfully updated to `{newPrefix}`.", "#00ffff")
            );
        }
    }
}
