namespace Questlog.Application.Common.DTOs.Member.Request;

public record CreateMemberRequestDto
{
    public string UserId { get; set; }
    public int CampaignId { get; set; }
}
