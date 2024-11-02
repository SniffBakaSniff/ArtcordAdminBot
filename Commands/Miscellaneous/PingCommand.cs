using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Commands.ContextChecks;
using ArtcordAdminBot.Features.Helpers;


namespace ArtcordAdminBot.Features
{
    public class PingCommand
    {

        [Command("ping")]
        public async Task PingAsync(CommandContext ctx)
        {
            await ctx.RespondAsync( MessageHelpers.GenericEmbed("","Pong!"));
        }
    }
}
