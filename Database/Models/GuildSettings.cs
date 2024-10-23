using System.ComponentModel.DataAnnotations;

public class GuildSettings
{
    [Key]
    public ulong GuildId { get; set; }  // Guild ID as the primary key
    public string Prefix { get; set; } = "!";  // Default prefix
}
