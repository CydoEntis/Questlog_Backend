using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.PartyMember;

public record UpdatePartyMemberRequestDTO
{
    [Required]
    public int Id { get; set; }
    public int CharacterId { get; set; }
    [Required]
    public int PartyId { get; set; }
    [Required]
    public int GuildId { get; set; }
    public string? Role { get; set; }
}
