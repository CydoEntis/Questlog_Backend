namespace Questlog.Application.Common.DTOs.GuildMember.Response;

public record CreateMemberRequestDto
{
    public string UserId { get; set; }
    public int CampaignId { get; set; }
}
