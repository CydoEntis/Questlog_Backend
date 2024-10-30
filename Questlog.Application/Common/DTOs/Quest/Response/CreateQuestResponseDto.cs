using Questlog.Application.Common.DTOs.Member.Response;
using Questlog.Application.Common.DTOs.Task.Response;

namespace Questlog.Application.Common.DTOs.Quest;

public record CreateQuestResponseDto()
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
    public List<CreateStepResponseDto> Steps { get; set; } = new List<CreateStepResponseDto>();
}