using Questlog.Application.Common.DTOs.GuildMember.Response;
using Questlog.Application.Common.DTOs.Subquest.Response;

namespace Questlog.Application.Common.DTOs.Quest;

public record CreateQuestResponseDto()
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Difficulty { get; set; }
    public bool isCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; } = DateTime.Now;
    public int CampaignId { get; set; }
    public List<GetMemberResponseDto> AssignedMembers { get; set; } = new List<GetMemberResponseDto>();
    public List<CreateSubquestResponseDto> Subquests { get; set; } = new List<CreateSubquestResponseDto>();

}