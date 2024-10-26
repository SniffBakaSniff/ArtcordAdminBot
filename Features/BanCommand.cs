using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Commands.ContextChecks;
using ArtcordAdminBot.Features.Helpers;
using System.IO;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace ArtcordAdminBot.Features
{
    public class BanCommand
    {
        private readonly IDatabaseService _databaseService;
        private readonly HttpClient _httpClient;

        public BanCommand(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _httpClient = new HttpClient();
        }

        [Command("ban")]
        [RequirePermissions(DiscordPermissions.BanMembers)] // Placeholder till we have a custom permission handler
        public async Task BanAsync(CommandContext ctx, 
        [System.ComponentModel.Description("The user to ban.")]DiscordUser targetUser, 
        [System.ComponentModel.Description("The reason for the ban.")] string? reason = null, 
        [System.ComponentModel.Description("The attachment of the reference image.")] DiscordAttachment? attachment = null, 
        [System.ComponentModel.Description("The ID of the reference message.")] ulong? referenceMessageId = null, 
        [System.ComponentModel.Description("Additional notes about the ban.")]string? internalNotes = null, 
        [System.ComponentModel.Description("Choose the timeframe for deleting user messages.")] MessageDeletionTimeframe deleteTimeframe = MessageDeletionTimeframe.None)
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

            await ctx.Guild.BanMemberAsync(targetUser, TimeSpan.FromDays((int)deleteTimeframe), reason);

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


            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
