using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using ArtcordAdminBot.Database;
using ArtcordAdminBot.Features;

namespace ArtcordAdminBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Load configuration from appsettings.json file.
            // This configuration file should contain the bot token and other settings.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
                .Build();

            // Create and configure the Discord client.
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = configuration["Token"],
                TokenType = TokenType.Bot, 
                Intents = DiscordIntents.AllUnprivileged
            });

            // Initialize the database
            await DatabaseHelper.InitializeDatabaseAsync();

            // Register the SlashCommands extension with the Discord client.
            var slash = discord.UseSlashCommands();

            // Register the CommandsModule class to handle slash commands.
            slash.RegisterCommands<CommandsModule>();
            slash.RegisterCommands<BanCommandsModule>();
            slash.RegisterCommands<CommandLogModule>();

            // Attach the OnReady event handler to the Discord client.
            discord.Ready += EventsModule.OnReady;

            // Connect the bot to Discord.
            await discord.ConnectAsync();
            
            // Keep the bot running indefinitely.
            await Task.Delay(-1);
        }
    }
}
