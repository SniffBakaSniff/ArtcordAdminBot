using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Features.AdminCommands
{   // This is the command group for the admin commands.
    [Command("admin")]
    [RequirePermissions(DiscordPermissions.Administrator)]
    public partial class AdminCommandGroup
    {

        private readonly IDatabaseService _databaseService;

        public AdminCommandGroup(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _httpClient = new HttpClient();
        }
    }
}
