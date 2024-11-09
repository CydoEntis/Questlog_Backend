using Questlog.Application.Common.DTOs.Step;

namespace Questlog.Application.Common.DTOs.Quest;

public class UpdateQuestDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int PartyId { get; set; }
    public List<int> MemberIds { get; set; }
    public List<UpdateStepDto> Steps { get; set; }
}