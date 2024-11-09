using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questlog.Domain.Entities;

public class UnlockedAvatar
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    [Required]
    public int AvatarId { get; set; }
    [ForeignKey("AvatarId")]
    public Avatar Avatar { get; set; }

    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
    public bool IsUnlocked { get; set; } = false;
}