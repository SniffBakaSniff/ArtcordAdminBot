using ArtcordAdminBot.Helpers;
using DSharpPlus.Commands;

using System.Threading.Tasks;

namespace ArtcordAdminBot.Features.ConfigCommands
{
    public partial class ConfigCommandsGroup
    {

        [Command("welcomemsg")]
        [System.ComponentModel.Description("Set the welcome message for new members")]
        public async Task SetWelcomeMessageAsync(CommandContext ctx, string welcomeMessage)
        {
            if (string.IsNullOrWhiteSpace(welcomeMessage))
            {
                await ctx.RespondAsync("Welcome message cannot be empty.");
                return;
            }

            await _messageSettingsService.ManageMessageSettingAsync(ctx.Guild!.Id, "welcome", welcomeMessage);
            await ctx.RespondAsync(
                MessageHelpers.GenericUpdateEmbed("Welcome Message Updated!\n", extra: welcomeMessage)
            );
        }

        [Command("farewellmsg")]
        [System.ComponentModel.Description("Set the farewell message for leaving members")]
        public async Task SetFarewellMessageAsync(CommandContext ctx, string farewellMessage)
        {
            if (string.IsNullOrWhiteSpace(farewellMessage))
            {
                await ctx.RespondAsync("Farewell message cannot be empty.");
                return;
            }

            await _messageSettingsService.ManageMessageSettingAsync(ctx.Guild!.Id, "farewell", farewellMessage);
            await ctx.RespondAsync(
                MessageHelpers.GenericUpdateEmbed("Farewell Message Updated!\n", extra: farewellMessage)
            );
        }
    }
}
