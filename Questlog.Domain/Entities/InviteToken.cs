namespace Questlog.Domain.Entities;

public class InviteToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public int CampaignId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedAt { get; set; }

    public DateTime Expiration { get; set; }
    public bool IsExpired { get; set; } = false;

    public virtual Party Party { get; set; }
}