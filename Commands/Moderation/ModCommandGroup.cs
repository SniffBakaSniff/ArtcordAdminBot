using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Features.ModerationCommands
{
    [Command("mod")]
    [RequirePermissions(DiscordPermissions.Administrator)] // Placeholder till custom permissions handler is in place
    public partial class ModerationCommandGroup
    {

        private readonly IBanService _banService;
        private readonly IGuildSettingsService _guildSettingsService;

        public ModerationCommandGroup(IBanService banService, IGuildSettingsService guildSettingsService)
        {
            _banService = banService;
            _guildSettingsService = guildSettingsService;
            _httpClient = new HttpClient();
        }
    }
}
