using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Listeners
{
    public class JoinLeaveListener
    {
        private readonly IDatabaseService _databaseService;

        public JoinLeaveListener(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task OnMemberJoined(DiscordClient client, GuildMemberAddedEventArgs e)
        {
            string? welcomeMessage = await _databaseService.ManageMessageSettingAsync(e.Guild.Id, "welcome");
            Console.WriteLine(welcomeMessage);
            ulong? welcomeChannelId = await _databaseService.GetWelcomeChannelAsync(e.Guild.Id);

            if (welcomeChannelId.HasValue && welcomeMessage != null)
            {
                var welcomeChannel = await e.Guild.GetChannelAsync(welcomeChannelId.Value);
                
                if (welcomeChannel is not null && welcomeChannel.Type == DiscordChannelType.Text)
                {
                    await welcomeChannel.SendMessageAsync(welcomeMessage.Replace("{server}", e.Guild.Name).Replace("{user}", e.Member.Mention));
                }
            }
        }

        public async Task OnMemberLeft(DiscordClient client, GuildMemberRemovedEventArgs e)
        {
            string? farewellMessage = await _databaseService.ManageMessageSettingAsync(e.Guild.Id, "farewell");
            ulong? farewellChannelId = await _databaseService.GetFarewellChannelAsync(e.Guild.Id);
            ulong? welcomeChannelId = await _databaseService.GetWelcomeChannelAsync(e.Guild.Id);
            
            if (farewellChannelId.HasValue && farewellMessage != null)
            {
                var farewellChannel = await e.Guild.GetChannelAsync(farewellChannelId.Value);
                var welcomeChannel = await e.Guild.GetChannelAsync(welcomeChannelId.Value);
                
                if (farewellChannel is not null && farewellChannel.Type == DiscordChannelType.Text)
                {
                    await farewellChannel.SendMessageAsync(farewellMessage.Replace("{server}", e.Guild.Name).Replace("{user}", e.Member.Mention));
                }
                if (farewellChannel is null && welcomeChannel is not null && welcomeChannel.Type == DiscordChannelType.Text)
                {
                    await welcomeChannel.SendMessageAsync(farewellMessage.Replace("{server}", e.Guild.Name).Replace("{user}", e.Member.Mention));
                }
            }
        }
    }
}
