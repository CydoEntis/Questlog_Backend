namespace Questlog.Application.Common.DTOs.GuildMember.Response;

public record UpdateMemberRoleResponseDto()
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int CampaignId { get; set; }
    public string Role { get; set; }
}