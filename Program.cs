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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
                .Build();

            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = configuration["Token"],
                TokenType = TokenType.Bot, 
                Intents = DiscordIntents.AllUnprivileged
            });

            // Initialize the database
            await DatabaseHelper.InitializeDatabaseAsync();


            var slash = discord.UseSlashCommands();

            slash.RegisterCommands<CommandsModule>();
            slash.RegisterCommands<BanCommandsModule>();
            slash.RegisterCommands<CommandLogModule>();

            // Attach the OnReady event handler to the Discord client.
            discord.Ready += EventsModule.OnReady;

            await discord.ConnectAsync();
            
            // Keep the bot running indefinitely.
            await Task.Delay(-1);
        }
    }
}
