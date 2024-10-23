namespace Questlog.Domain.Entities;

public class MemberQuest
{
    public int AssignedMemberId { get; set; }
    public Member AssignedMember { get; set; }

    public int AssignedQuestId { get; set; }
    public Quest AssignedQuest { get; set; }
}