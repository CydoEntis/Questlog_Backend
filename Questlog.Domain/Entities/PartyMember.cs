using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questlog.Domain.Entities;

public class PartyMember
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }

    public int GuildId { get; set; }

    [ForeignKey("GuildMember")]
    public int GuildMemberId { get; set; } 

    public GuildMember GuildMember { get; set; }

    [ForeignKey("Party")]
    public int PartyId { get; set; } 

    public Party Party { get; set; } 

    public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string Role { get; set; } = "Member";
}
