using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.PartyMember;

public record CreatePartyMemberRequestDto
{
    public string UserId { get; set; }
    public int GuildId { get; set; }
    public int GuildMemberId { get; set; }
}
