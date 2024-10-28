using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.EventArgs;
using System.ComponentModel;

namespace ArtcordAdminBot.Features.ModerationCommands
{
    public partial class ModerationCommandGroup
    {
        private readonly HttpClient _httpClient;


        [Command("ban")]
        [RequirePermissions(DiscordPermissions.BanMembers)] 
        // Placeholder till we have a custom permission handler. Whenever we get a custom permission handler we can just handle permissions on a per group basis.
        public async Task BanAsync(CommandContext ctx,
        [Description("The user to ban.")]DiscordUser targetUser, 
        [Description("The reason for the ban.")] string? reason = null, 
        [Description("The attachment of the reference image.")] DiscordAttachment? attachment = null, 
        [Description("The ID of the reference message.")] ulong? referenceMessageId = null, 
        [Description("Additional notes about the ban.")]string? internalNotes = null, 
        [Description("Choose the timeframe for deleting user messages.")] MessageDeletionTimeframe deleteTimeframe = MessageDeletionTimeframe.None)
        {
            // Attachment processing
            string? referenceImagePath = null;
            if (attachment != null)
            {
                // Download and save the image
                string fileName = $"ban_reference_{ctx.Guild!.Id}_{targetUser.Id}_{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss.fff")}.png";
                string filePath = Path.Combine("Images", fileName);
                
                Directory.CreateDirectory("Images");
                
                using (var imageStream = await _httpClient.GetStreamAsync(attachment.Url))
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await imageStream.CopyToAsync(fileStream);
                }

                referenceImagePath = filePath;
            }

            // Create the ban record in the database
            await _databaseService.NewBanRecordAsync(
                guildId: ctx.Guild!.Id,
                userId: targetUser.Id,
                moderatorId: ctx.User.Id,
                reason: reason,
                referenceImagePath: referenceImagePath,
                referenceMessageId: referenceMessageId,
                internalNotes: internalNotes
            );

            await ctx.Guild.BanMemberAsync(targetUser, TimeSpan.FromHours((int)deleteTimeframe), reason);

            var embed = new DiscordEmbedBuilder
            {
                Title = "User Banned",
                Color = DiscordColor.Red,
                Description = $"**User:** {targetUser.Username}\n" +
                              $"**Reason:** {reason}\n" +
                              $"**Moderator:** {ctx.User.Username}",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = targetUser.AvatarUrl
                },
                Timestamp = DateTime.UtcNow
            };

            var dmEmbed = new DiscordEmbedBuilder
            {
                Title = "You Have Been Banned",
                Color = DiscordColor.Red,
                Description = $"**Server:** {ctx.Guild.Name}\n" +
                            $"**Reason:** {reason}\n" +
                            $"**Appeals:** If you believe this ban was a mistake, please reach out to the moderation team for clarification.",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = targetUser.AvatarUrl
                },
                Timestamp = DateTime.UtcNow
            };

            await targetUser.SendMessageAsync(new DiscordMessageBuilder()
                .AddEmbeds([dmEmbed.Build()])
                .AddComponents(new DiscordButtonComponent(DiscordButtonStyle.Primary, "appeal_ban", "Appeal Ban"))
            );

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}