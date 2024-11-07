namespace Questlog.Domain.Entities;

public class Quest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; } = DateTime.Now;

    public DateTime? CompletionDate { get; set; } = null;
    
    public int PartyId { get; set; }
    public Party Party { get; set; }

    public virtual List<Step> Steps { get; set; } = new List<Step>();
    
    public ICollection<MemberQuest> MemberQuests { get; set; } = new List<MemberQuest>();
}