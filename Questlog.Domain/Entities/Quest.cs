namespace Questlog.Domain.Entities;

public class Quest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; } = DateTime.Now;

    public int CampaignId { get; set; }
    public Campaign Campaign { get; set; }

    public virtual List<Task> Tasks { get; set; } = new List<Task>();
    
    public ICollection<MemberQuest> MemberQuests { get; set; } = new List<MemberQuest>();
}