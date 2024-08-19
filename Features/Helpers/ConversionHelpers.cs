using DSharpPlus.Commands;
using DSharpPlus.Entities;
using System.Text;

namespace ArtcordAdminBot.Features.Helpers
{
    /// <summary>
    /// Converting string arguments into the correct types
    /// </summary>
    public static class ConversionHelpers
    {
        public enum Type
        {
            Message
        }

        public class Exception : System.Exception
        {
            public new string? Message;
            public string Argument;
            public Type Type;

            public Exception(string? message, string argument, Type type) {
                Message = message; Argument = argument; Type = type;
            }

            public DiscordEmbed ToEmbed()
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine($"Error when attempting to parse argument `{Argument}`.");
                if (Message != null)
                    message.AppendLine($"> {Message}");
                switch (Type)
                {
                    case Type.Message:
                        message.AppendLine("*A message link or message ID must be provided.*");
                        break;
                }

                return MessageHelpers.GenericErrorEmbed(title: "Parsing Error", message: message.ToString());
            }
        }

        /// <summary>
        /// Converts a message link or ID into the ID
        /// </summary>
        public static ulong GetMessageId(string s, string argumentName, CommandContext context)
        {
            if (ulong.TryParse(s, out ulong messageId))
            {
                return messageId;
            }

            string tryString = $"https://discord.com/channels/{context.Guild.Id}/{context.Channel.Id}/";
            if (s.StartsWith(tryString))
            {
                return GetMessageId(s.Substring(tryString.Length), argumentName, context);
            }

            // Just for better error messages
            tryString = $"https://discord.com/channels/{context.Guild.Id}/";
            if (s.StartsWith(tryString))
            {
                throw new Exception($"The message must be of the same channel ({context.Channel.Mention}).", argumentName, Type.Message);
            }
            tryString = $"https://discord.com/channels/";
            if (s.StartsWith(tryString))
            {
                throw new Exception($"The message must be of the same server and channel.", argumentName, Type.Message);
            }

            throw new Exception(null, argumentName, Type.Message);
        }
    }
}
