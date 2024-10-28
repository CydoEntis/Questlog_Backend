using Questlog.Application.Common.DTOs.Task.Request;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.DTOs.Quest.Request;

public record CreateQuestRequestDto()
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int CampaignId { get; set; }
    public List<int> MemberIds { get; set; }
    public List<CreateTaskRequestDto> Tasks { get; set; }
}