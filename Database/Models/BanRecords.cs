using System.ComponentModel.DataAnnotations;

public class BanRecords
{
    [Key]
    public int BanId { get; set; }

    public ulong GuildId { get; set; }

    public ulong UserId { get; set; }

    public ulong ModeratorId { get; set; }

    [MaxLength(500)]
    public string Reason { get; set; } = "None";

    public string ReferenceImagePath { get; set; } = "None";

    public ulong? ReferenceMessageId { get; set; }

    public DateTime BanDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpirationDate { get; set; }
    [MaxLength(50)]
    public string AppealStatus { get; set; } = "None";

    public DateTime? AppealDate { get; set; }

    [MaxLength(1000)]
    public string AdditionalNotes { get; set; } = "None";
}