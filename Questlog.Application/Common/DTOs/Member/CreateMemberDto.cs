namespace Questlog.Application.Common.DTOs.Member;

public class CreateMemberDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int CampaignId { get; set; }
    public string Role { get; set; }
}