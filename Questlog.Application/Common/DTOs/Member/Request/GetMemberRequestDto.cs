namespace Questlog.Application.Common.DTOs.Member.Request;

public record GetMemberRequestDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int CampaignId { get; set; }
    public string Role { get; set; }
}