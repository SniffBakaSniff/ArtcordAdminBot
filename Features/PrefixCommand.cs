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
        [RequirePermissions(DiscordPermissions.Administrator)]
        public async Task SetPrefixAsync(CommandContext ctx, string newPrefix)
        {
            if (string.IsNullOrWhiteSpace(newPrefix))
            {
                await ctx.RespondAsync("Prefix cannot be empty.");
                return;
            }

            if (newPrefix.Length > 3) { 
                await ctx.RespondAsync("Prefix cannot be longer than 3 characters.");
                return;
            }

            newPrefix = newPrefix.ToLower();
            await _databaseService.SetPrefixAsync(ctx.Guild.Id, newPrefix);

            await ctx.RespondAsync(
                MessageHelpers.GenericSuccessEmbed("Prefix updated", $"Prefix successfully updated to `{newPrefix}`.")
            );
        }
    }
}
