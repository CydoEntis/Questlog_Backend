namespace Questlog.Application.Common.DTOs.Guild.Requests;

public record UpdateCampaignOwnerRequestDto
{
    public int CampaignId { get; set; }
    public string UserId { get; set; }
}
