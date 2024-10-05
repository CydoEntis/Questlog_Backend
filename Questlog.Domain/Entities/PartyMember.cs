using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Questlog.Domain.Entities
{
    public class PartyMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("CharacterId")]
        public Character Character { get; set; }

        [ForeignKey("Party")]
        public int PartyId { get; set; }

        public Party Party { get; set; }

        public IdentityRole Role { get; set; }

        public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
