using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Domain.Entities
{
    public class GuildMember
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Guild")]
        public int GuildId { get; set; }

        public Guild Guild { get; set; }

        public DateTime JoinedOn { get; set; } = DateTime.Now;
    }
}
