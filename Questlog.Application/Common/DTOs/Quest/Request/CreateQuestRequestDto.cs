using Questlog.Application.Common.DTOs.Task.Request;
using Questlog.Application.Common.DTOs.Task.Response;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.DTOs.Quest.Request;

public record CreateQuestRequestDto()
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int CampaignId { get; set; }
    public List<int> MemberIds { get; set; }
    public List<CreateStepRequestDto> Steps { get; set; }
}