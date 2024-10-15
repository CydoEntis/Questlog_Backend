using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questlog.Domain.Entities;

public class GuildMember
{
    [Key]
    public int Id { get; set; }


    [ForeignKey("Guild")]
    public int GuildId { get; set; }

    public Guild Guild { get; set; }

    public string Role { get; set; }

    public DateTime JoinedOn { get; set; } = DateTime.Now;
    public DateTime UpdatedOn { get; set; }

    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }
}
