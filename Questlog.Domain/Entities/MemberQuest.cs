namespace Questlog.Domain.Entities;

public class MemberQuest
{
    public int AssignedMemberId { get; set; }
    public Member AssignedMember { get; set; }

    public int AssignedQuestId { get; set; }
    public Quest AssignedQuest { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public bool IsCompleted { get; set; } = false;
    public DateTime CompletionDate { get; set; }
    public int AwardedExp { get; set; } = 0;
    public int AwardedCurrency { get; set; } = 0;
}