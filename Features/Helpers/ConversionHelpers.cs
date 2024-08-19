using DSharpPlus.Commands;

namespace ArtcordAdminBot.Features.Helpers
{
    /// <summary>
    /// Converting string arguments into the correct types
    /// </summary>
    public static class ConversionHelpers
    {
        /// <summary>
        /// Converts a message link or ID into the ID
        /// </summary>
        public static ulong GetMessageId(string s, CommandContext context)
        {
            if (ulong.TryParse(s, out ulong messageId))
            {
                return messageId;
            }
            return 0;

            //if (s.StartsWith())
        }
    }
}
