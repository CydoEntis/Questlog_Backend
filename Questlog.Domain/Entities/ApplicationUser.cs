using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Questlog.Domain.Entities;

public class ApplicationUser : IdentityUser
{

    [Required]
    [MaxLength(12)]
    [MinLength(3)]
    public string DisplayName { get; set; }

    [Required]
    public Avatar Avatar { get; set; }

    [Required]
    public int CurrentLevel { get; set; } = 1;

    [Required]
    public int CurrentExp { get; set; } = 0;

    [Required]
    public int ExpToNextLevel { get; set; } = 100;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Guild> Guilds { get; set; } = new List<Guild>();

    public virtual ICollection<GuildMember> GuildMembers { get; set; } = new List<GuildMember>();


    public int CalculateExpForLevel()
    {
        int baseExp = 100;
        return baseExp * CurrentLevel;
    }



}
