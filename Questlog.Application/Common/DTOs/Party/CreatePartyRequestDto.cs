using System.ComponentModel.DataAnnotations;
using Questlog.Application.Common.DTOs.GuildMember.Response;
using Questlog.Application.Common.DTOs.PartyMember;

namespace Questlog.Application.Common.DTOs.Party;

public record CreatePartyRequestDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; }

    public string Color { get; set; }
    [Required]
    public int GuildId { get; set; }
}
