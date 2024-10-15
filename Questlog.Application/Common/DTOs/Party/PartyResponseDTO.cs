using Questlog.Application.Common.DTOs.PartyMember;
using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.Party;

public record PartyResponseDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; }

    [Required]
    public int GuildId { get; set; }

    public virtual List<PartyMemberResponseDTO> PartyMembers { get; set; } = new List<PartyMemberResponseDTO>();
}
