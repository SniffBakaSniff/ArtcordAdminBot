using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Features.ConfigCommands
{
    [Command("config")]
    [RequirePermissions(DiscordPermissions.Administrator)]
    public partial class ConfigCommandsGroup
    {

        private readonly IGuildSettingsService _guildSettingsService;
        private readonly IMessageSettingsService _messageSettingsService;

        public ConfigCommandsGroup(IGuildSettingsService guildSettingsService, IMessageSettingsService messageSettingsService)
        {
            _guildSettingsService = guildSettingsService;
            _messageSettingsService = messageSettingsService;
        }
    }
}
