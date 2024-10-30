using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

using ArtcordAdminBot.Features.Helpers;
using DSharpPlus.Commands;

using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Features.ConfigCommands
{
    public partial class ConfigCommandsGroup
    {

        [Command("welcomechannel")]
        [System.ComponentModel.Description("Set the welcome message for new members")]
        public async Task SetWelcomeChannelAsync(CommandContext ctx, DiscordChannel channel)
        {
            if (channel is null)
            {
                await ctx.RespondAsync("Please Select a Channel");
                return;
            }
            if (channel.Type != DiscordChannelType.Text)
            {
                await ctx.RespondAsync("Please Select a Text Channel");
                return;
            }

            await _databaseService.SetWelcomeChannelAsync(ctx.Guild!.Id, channel.Id);
            await ctx.RespondAsync(
                MessageHelpers.GenericUpdateEmbed("Welcome Channel Updated!\n", extra: channel.Mention)
            );
        }

        [Command("farewellchannel")]
        [System.ComponentModel.Description("Set the farewell message for leaving members")]
        public async Task SetFarewellChannelAsync(CommandContext ctx, DiscordChannel channel)
        {
            if (channel is null)
            {
                await ctx.RespondAsync("Please Select a Channel");
                return;
            }
            if (channel.Type != DiscordChannelType.Text)
            {
                await ctx.RespondAsync("Please Select a Text Channel");
                return;
            }

            await _databaseService.SetFarewellChannelAsync(ctx.Guild!.Id, channel.Id);
            await ctx.RespondAsync(
                MessageHelpers.GenericUpdateEmbed("Farewell Message Updated!\n", extra: channel.Mention)
            );
        }
    }
}
