using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Features.ConfigCommands
{   // This is the command group for the config commands.
    [Command("config")]
    [RequirePermissions(DiscordPermissions.Administrator)]
    public partial class ConfigCommandsGroup
    {

        private readonly IDatabaseService _databaseService;

        public ConfigCommandsGroup(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
    }
}
