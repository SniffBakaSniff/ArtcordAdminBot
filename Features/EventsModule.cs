using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace ArtcordAdminBot.Features
{
    /// <summary>
    /// Handles events for the Discord bot.
    /// </summary>
    public class EventsModule
    {
        /// <summary>
        /// Gets triggered when the bot is ready and connected to Discord.
        /// </summary>
        /// <param name="sender">The instance of the bot client</param>
        /// <param name="e">Event-specific data</param>
        public static Task OnReady(DiscordClient sender, ReadyEventArgs e)
        {
            Console.WriteLine($"Logged in as {sender.CurrentUser.Username}");
            
            return Task.CompletedTask;
        }
    }
}
