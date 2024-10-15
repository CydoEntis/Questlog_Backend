using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.PartyMember;

public record CreatePartyMemberRequestDTO
{
    [Required]
    public int CharacterId { get; set; }
    [Required]
    public int PartyId { get; set; }
    [Required]
    public int GuildId { get; set; }
    [Required]
    public string Role { get; set; }
}
