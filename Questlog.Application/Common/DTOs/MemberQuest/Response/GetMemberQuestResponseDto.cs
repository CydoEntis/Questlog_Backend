namespace Questlog.Application.Common.DTOs.MemberQuest.Response;

public record GetMemberQuestResponseDto()
{
    public int AssignedMemberId { get; set; }
    public int AssignedQuestId { get; set; }
    public string UserId { get; set; }
};