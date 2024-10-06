using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Party
{
    public record CreatedPartyResponseDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int GuildId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CreatedPartyMemberResponseDTO> PartyMembers { get; set; } = new List<CreatedPartyMemberResponseDTO>();
    }
}
