namespace Questlog.Application.Common.DTOs.GuildMember.Response;

public record GetMemberResponseDto
{
    public int Id { get; set; }
    public int CampaignId { get; set; }
    public string Role { get; set; }
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public int Avatar { get; set; }
    public int CurrentLevel { get; set; }
    public DateTime JoinedOn { get; set; }
    
}
