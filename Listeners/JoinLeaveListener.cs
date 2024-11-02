using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace ArtcordAdminBot.Listeners
{
    public class JoinLeaveListener
    {
        private readonly IMessageSettingsService _ticketSettingsService;
        private readonly IGuildSettingsService _guildSettingsService;

        public JoinLeaveListener(IMessageSettingsService messageSettingsService, IGuildSettingsService guildSettingsService)
        {
            _ticketSettingsService = messageSettingsService;
            _guildSettingsService = guildSettingsService;
        }

        public async Task OnMemberJoined(DiscordClient client, GuildMemberAddedEventArgs e)
        {
            string? welcomeMessage = await _ticketSettingsService.ManageMessageSettingAsync(e.Guild.Id, "welcome");
            Console.WriteLine(welcomeMessage);
            ulong? welcomeChannelId = await _guildSettingsService.GetWelcomeChannelAsync(e.Guild.Id);

            if (welcomeChannelId.HasValue && welcomeMessage != null)
            {
                var welcomeChannel = await e.Guild.GetChannelAsync(welcomeChannelId.Value);
                
                if (welcomeChannel is not null && welcomeChannel.Type == DiscordChannelType.Text)
                {
                    await welcomeChannel.SendMessageAsync(welcomeMessage.Replace("{server}", e.Guild.Name).Replace("{user}", e.Member.Mention));
                }
            }
        }

        public async Task OnMemberLeft(GuildMemberRemovedEventArgs e)
        {
            string? farewellMessage = await _ticketSettingsService.ManageMessageSettingAsync(e.Guild.Id, "farewell");
            ulong? farewellChannelId = await _guildSettingsService.GetFarewellChannelAsync(e.Guild.Id);
            ulong? welcomeChannelId = await _guildSettingsService.GetWelcomeChannelAsync(e.Guild.Id);
            
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
