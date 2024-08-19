using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.SlashCommands;
using ArtcordAdminBot.Features;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;

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

            DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(discordToken, TextCommandProcessor.RequiredIntents | SlashCommandProcessor.RequiredIntents | DiscordIntents.MessageContents);

            // Use the commands extension
            builder.UseCommands
            (
                // we register our commands here
                extension =>
                {
                    extension.AddCommands([typeof(EchoCommand), typeof(PurgeCommand)]);
                    TextCommandProcessor textCommandProcessor = new(new()
                    {
                        PrefixResolver = new DefaultPrefixResolver(true, "?", ".").ResolvePrefixAsync
                    });

                    // Add text commands with a custom prefix (?ping)
                    extension.AddProcessors(textCommandProcessor);

                    extension.CommandErrored += EventHandlers.CommandErrored;
                },
                new CommandsConfiguration()
                {
                    DebugGuildId = 1219490918235901962,
                    // The default value, however it's shown here for clarity
                    RegisterDefaultCommandProcessors = true,
                    UseDefaultCommandErrorHandler = false
                }
            );


            DiscordClient client = builder.Build();

            // We can specify a status for our bot. Let's set it to "playing" and set the activity to "with fire".
            DiscordActivity status = new("with fire", DiscordActivityType.Playing);

            // Now we connect and log in.
            await client.ConnectAsync(status, DiscordUserStatus.Online);

            // And now we wait infinitely so that our bot actually stays connected.
            await Task.Delay(-1);
        }

    }
}
