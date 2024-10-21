using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;

public class CustomPrefixResolver : IPrefixResolver
{
    private readonly IDatabaseService _database;

    public CustomPrefixResolver(IDatabaseService database)
    {
        _database = database;
    }

    public async ValueTask<int> ResolvePrefixAsync(CommandsExtension extension, DiscordMessage message)
    {
        if (string.IsNullOrWhiteSpace(message.Content) || message.Channel == null)
        {
            return -1;
        }

        if (message.Channel.GuildId.HasValue)
        {
            var guildId = message.Channel.GuildId.Value;
            var prefix = await _database.GetPrefixAsync(guildId);
            if (message.Content.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return prefix.Length;
            }
        }

        return -1;
    }
}
