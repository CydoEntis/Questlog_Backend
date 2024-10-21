namespace Questlog.Domain.Entities;

public class Quest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Difficulty { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; } = DateTime.Now;

    public int CampaignId { get; set; }
    public Campaign Campaign { get; set; }

    public virtual ICollection<Member> AssignedMembers { get; set; } = new List<Member>();

    public virtual List<Subquest> Subquests { get; set; } = new List<Subquest>();
}