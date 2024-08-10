using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace ArtcordAdminBot.Features
{
    // EventsModule class handles events for the Discord bot.
    public class EventsModule
    {
        // OnReady is an event handler that gets triggered when the bot is ready and connected to Discord.
        public static Task OnReady(DiscordClient sender, ReadyEventArgs e)
        {
            // Print a message to the console indicating the bot has successfully logged in and is ready.
            // 'sender' refers to the instance of the bot client, and 'e' contains event-specific data.
            Console.WriteLine($"Logged in as {sender.CurrentUser.Username}");
            
            // Return a completed task to signify that the event handling is done.
            return Task.CompletedTask;
        }
    }
}
