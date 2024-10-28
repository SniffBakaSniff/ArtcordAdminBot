using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Features.ModerationCommands
{
    [Command("moderation")]
    [RequirePermissions(DiscordPermissions.Administrator)] // Placeholder till custom permissions handler is in place
    public partial class ModerationCommandGroup
    {

        private readonly IDatabaseService _databaseService;

        public ModerationCommandGroup(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _httpClient = new HttpClient();
        }
    }
}
