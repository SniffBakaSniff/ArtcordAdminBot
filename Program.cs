using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.SlashCommands;
using ArtcordAdminBot.Features;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using ArtcordAdminBot.Features.ConfigCommands;
using ArtcordAdminBot.Features.AdminCommands;

namespace ArtcordAdminBot
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string? discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                Console.WriteLine("Error: No discord token found. Please provide a token via the DISCORD_TOKEN environment variable.");
                Environment.Exit(1);
            }

            DiscordClientBuilder builder = DiscordClientBuilder
                .CreateDefault(discordToken, TextCommandProcessor.RequiredIntents | SlashCommandProcessor.RequiredIntents | DiscordIntents.MessageContents)
                .ConfigureServices(services => 
                {
                    services.AddDbContext<BotDbContext>();
                    services.AddScoped<IPrefixResolver, CustomPrefixResolver>();
                    services.AddScoped<IDatabaseService, DatabaseService>();
                });

            // Use the commands extension
            builder.UseCommands
            (
                // we register our commands here
                extension =>
                {
                    extension.AddCommands([typeof(EchoCommand), typeof(PingCommand), typeof(ConfigCommandsGroup), typeof(AdminCommandGroup)]);
                    TextCommandProcessor textCommandProcessor = new(new TextCommandConfiguration
                    {
                       // PrefixResolver = new DefaultPrefixResolver(true, "?", ".").ResolvePrefixAsync
                    });

                    

                    // Add text commands with a custom prefix (?ping)
                    extension.AddProcessors(textCommandProcessor);

                    extension.CommandErrored += EventHandlers.CommandErrored;
                },

                
                new CommandsConfiguration()
                {
                    DebugGuildId = 1219490918235901962,
                    RegisterDefaultCommandProcessors = true,
                    UseDefaultCommandErrorHandler = false
                }
            );


            DiscordClient client = builder.Build();

            DiscordActivity status = new("Subaka", DiscordActivityType.ListeningTo);

            await client.ConnectAsync(status, DiscordUserStatus.Online);

            await Task.Delay(-1);
        }

    }
    
}
