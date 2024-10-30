using Questlog.Application.Common.DTOs.Member.Response;
using Questlog.Application.Common.DTOs.MemberQuest.Response;
using Questlog.Application.Common.DTOs.Task.Response;

namespace Questlog.Application.Common.DTOs.Quest;

public record GetQuestResponseDto()
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public bool isCompleted { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public int CampaignId { get; set; }
    public List<GetMemberResponseDto> Members { get; set; } = new List<GetMemberResponseDto>();
    public List<GetStepResponseDto> Steps { get; set; } = new List<GetStepResponseDto>();
    public int CompletedSteps { get; set; }
}