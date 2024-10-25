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
        [RequirePermissions(DiscordPermissions.BanMembers)]
        public async Task BanAsync(CommandContext ctx, DiscordUser targetUser, string reason = "No reason provided", DiscordAttachment? attachment = null, ulong? referenceMessageId = null, string additionalNotes = "None", bool deleteMessages = false)
        {
            var time = TimeSpan.FromDays(7);
            
            // Attachment processing
            string referenceImagePath = "None";
            if (attachment != null)
            {
                // Download and save the image
                string fileName = $"ban_reference_{targetUser.Id}_{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss")}.png";
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
                guildId: ctx.Guild.Id,
                userId: targetUser.Id,
                moderatorId: ctx.User.Id,
                reason: reason,
                referenceImagePath: referenceImagePath,
                referenceMessageId: referenceMessageId,
                additionalNotes: additionalNotes
            );

            if (!deleteMessages)
            {
                time = TimeSpan.FromDays(0);
            }

            await ctx.Guild.BanMemberAsync(targetUser, time, reason);

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
