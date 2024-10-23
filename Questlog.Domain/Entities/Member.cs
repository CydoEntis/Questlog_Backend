using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questlog.Domain.Entities;

public class Member
{
    [Key]
    public int Id { get; set; }


    [ForeignKey("Campaign")]
    public int CampaignId { get; set; }

    public Campaign Campaign { get; set; }

    public string Role { get; set; }

    public DateTime JoinedOn { get; set; } = DateTime.Now;
    public DateTime UpdatedOn { get; set; } = DateTime.Now;

    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }
    
    public virtual ICollection<Quest> AssignedQuests { get; set; } = new List<Quest>();
    
    public ICollection<MemberQuest> MemberQuests { get; set; } = new List<MemberQuest>();
}
