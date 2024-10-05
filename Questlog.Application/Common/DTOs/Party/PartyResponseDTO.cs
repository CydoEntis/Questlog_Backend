using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questlog.Application.Common.DTOs.PartyMember;

namespace Questlog.Application.Common.DTOs.Party
{
    public class PartyResponseDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        public int GuildId { get; set; }

        public virtual List<PartyMemberResponseDTO> PartyMembers { get; set; } = new List<PartyMemberResponseDTO>();
    }
}
