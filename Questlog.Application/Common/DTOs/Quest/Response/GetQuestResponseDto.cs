using Questlog.Application.Common.DTOs.Member.Response;
using Questlog.Application.Common.DTOs.MemberQuest.Response;
using Questlog.Application.Common.DTOs.Task.Response;

namespace Questlog.Application.Common.DTOs.Quest;

public record GetQuestResponseDto()
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Difficulty { get; set; }
    public bool isCompleted { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public int CampaignId { get; set; }
    public List<GetMemberResponseDto> AssignedMembers { get; set; } = new List<GetMemberResponseDto>();
    public int TotalMembers { get; set; }
    public List<GetTaskResponseDto> Tasks { get; set; } = new List<GetTaskResponseDto>();
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
}