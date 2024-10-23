using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questlog.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public int CampaignId { get; set; }
    public string Role { get; set; }
    public DateTime JoinedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    public Campaign Campaign { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<MemberQuest> MemberQuests { get; set; } // If applicable
}